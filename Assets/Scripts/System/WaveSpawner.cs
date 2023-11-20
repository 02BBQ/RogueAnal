using Assets.Scripts.PowerUp;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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

    public int CurrentWave = 0;

    private void Start()
    {
        CalculateWeights();
        body = Player.Instance.transform;
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
                break;
            }
            yield return null; // waiting until player choose power buff
        }
        teleport = false;
        PowerUped?.Invoke();
        Hades_Transition.Instance.targetValue = 1f;
        yield return new WaitForSeconds(.5f);
        Hades_Transition.Instance.targetValue = -.1f;
        yield return new WaitForSeconds(.5f);
        CurrentWave++;
        enemyCount += Random.Range(1, 3);
        for (int i = 0; i < enemyCount; i++)
        {
            EnemySO randomEnemy = enemies[GetRandomEnemyIndex()];
            var enemyGO = PoolManager.Get(randomEnemy.Prefab, SpawnEnemyPosition(), Quaternion.identity,transform);
            PoolManager.Get(spawnFX, enemyGO.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    private void OnTransformChildrenChanged()
    {
        if (transform.childCount <= 0)
        {
            waitingForInput = true;
            Cleared?.Invoke();
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