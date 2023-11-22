using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class WaveSpawner : Singleton<WaveSpawner>
{
    [SerializeField] private Transform spawner;
    private System.Random rand = new System.Random();
    private double accumulatedWeights;
    private int maxAttempts = 20;
    
    private float spawnAreaRadius = 11f; 
    private float playerSafeRadius = 3.5f; 

    private Transform body;

    public bool waitingForInput = false;
    public bool teleport = false;

    [SerializeField] private GameObject powerUp;

    [SerializeField] private EnemySO[] enemies;

    private int enemyCount = 2;

    [SerializeField] GameObject spawnFX;

    public UnityEvent Cleared;
    public UnityEvent Black;
    public UnityEvent PowerUped;
    [SerializeField] private GameObject prestigeLoopUI;

    public int CurrentWave = 0;
    public int TotalWave = 0;

    public TMP_Text waveText;

    [SerializeField] private bool prestigeLoop = false;

    private float shopChance = 0;
    public float ShopChance
    {
        get { return shopChance; }
        set
        {
            shopChance = Mathf.Clamp(value, 0, 100);
        }
    }
    private int noShopInARow = 0;

    public Volume globalVolume;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    [SerializeField] private GameObject shop;

    [SerializeField] private AudioSource globalSound;
    [SerializeField] private AudioClip loopd;

    private void Start()
    {
        if (globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        if (globalVolume.profile.TryGet<LensDistortion>(out lensDistortion))
        CalculateWeights();
        try
        {
            body = Player.Instance.transform;
        }
        catch 
        {
            body = GameObject.FindWithTag("Player").transform;
        }
        StartWave();
    }
            
    private void StartWave()
    {
        StartCoroutine(DoWave());
    }

    public Vector3 SpawnEnemyPosition()
    {
        Vector3 spawnPosition;
        int currentAttempt = 0;

        do
        {
            // get random pos on range
            float x = Random.Range(-spawnAreaRadius, spawnAreaRadius);
            float z = Random.Range(-spawnAreaRadius, spawnAreaRadius);
            spawnPosition = new Vector3(x, 0.5f, z);

            currentAttempt++;
            if (currentAttempt > maxAttempts) break; // break loop when did max attemp
        } while (Vector3.Distance(spawnPosition, body.position) < playerSafeRadius); // check distance
        return spawnPosition;
    }

    IEnumerator DoWave()
    {
        while (waitingForInput)
        {
            yield return null; // waiting until player choose power buff
        }

        PoolManager.Get(powerUp, Player.Instance.transform.position,Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(25, 1f);
        Teleporter.Instance.gameObject.SetActive(true);
        while (!teleport)
        {
            if (Input.GetKeyDown(KeyCode.F) && Teleporter.Instance.entered)
            {
                Teleporter.Instance.PressF?.Invoke();
                Teleporter.Instance.entered = false;
                shop.SetActive(false);
                break;
            }
            yield return null; // waiting until player choose power buff
        }
        teleport = false;
        PowerUped?.Invoke();
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 1, .15f).SetEase(Ease.OutQuad).OnComplete(() => DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0, .4f).SetEase(Ease.InQuad));
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, .7f, .25f).SetEase(Ease.OutQuad).OnComplete(() => DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0, .6f).SetEase(Ease.InQuad));
        CinemachineShake.Instance.ShakeCamera(5, 11);
        Hades_Transition.Instance.targetValue = 1f;
        yield return new WaitForSeconds(.5f);
        Hades_Transition.Instance.targetValue = -.1f;
        yield return new WaitForSeconds(.5f);
        CurrentWave++;
        TotalWave++;
        noShopInARow++;
        // prestigeLoop
        waveText.text = $"Wave : {CurrentWave}";
        if(TotalWave % 10 == 0)
        {
            prestigeLoopUI.SetActive(true);
            do
            {
                yield return null;
            } while (!Input.GetKey(KeyCode.N) && !Input.GetKey(KeyCode.Y)); 
            if (Input.GetKeyDown(KeyCode.Y)) // yes
            {
                waveText.text = $"Wave : {CurrentWave}";
                CurrentWave = 0;
                enemyCount = 2;
                prestigeLoop = true;
                globalSound.PlayOneShot(loopd);
            }
            BuffEnemies();
            CinemachineShake.Instance.ShakeCamera(30f, 2);
            DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 1, .15f).SetEase(Ease.OutQuad).OnComplete(() => DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0, 2f).SetEase(Ease.InQuad));
            DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 1, .25f).SetEase(Ease.OutQuad).OnComplete(() => DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0, 1.8f).SetEase(Ease.InQuad));
            prestigeLoopUI.SetActive(false);
        }
        
        if (!prestigeLoop)
        {
            if (shopChance > Random.Range(0, 100))
            {
                noShopInARow = 0;
                ShopChance = 0;
                shop.SetActive(true);
                StartCoroutine(DoWave());
            }
            else
            {

                enemyCount += Random.Range(1, 3);
                for (int i = 0; i < enemyCount; i++)
                {
                    EnemySO randomEnemy = enemies[GetRandomEnemyIndex()];
                    var enemyGO = PoolManager.Get(randomEnemy.Prefab, SpawnEnemyPosition(), Quaternion.identity, transform);
                    PoolManager.Get(spawnFX, enemyGO.transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(.1f);
                }
                yield return null;
            }
        }
        else
        {
            StartCoroutine(DoWave());
            prestigeLoop = false;
        }
        ShopChance += 7.7777f + Mathf.PI * noShopInARow;
    }

    private void OnTransformChildrenChanged()
    {
        if (transform.childCount <= 0)
        {
            waitingForInput = true;
            Cleared?.Invoke();
            ShopChance += 10f + Mathf.PI * noShopInARow;
            StartCoroutine(DoWave());
        }
    }

    private int GetRandomEnemyIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < enemies.Length; i++)
            if (enemies[i]._weight >= r)
                return i;

        return 0;
    }

    private void BuffEnemies()
    {
        GameManager.BuffVal++;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (EnemySO enemy in enemies)
        {
            accumulatedWeights += enemy.Chance;
            enemy._weight = accumulatedWeights;
        }
    }
}