using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Player : Singleton<Player>, IDamageable
{
    public enum State
    {
        None,
        IFrame,
        Dead,
    }

    [Header("Instance")]
    [SerializeField] private GameObject bullet;
    public Rigidbody rb;
    [SerializeField] private SpriteRenderer rbSprite;
    [SerializeField] private Sprite cumMaterial;
    [SerializeField] private Sprite defMaterial;

    [SerializeField] private GameObject damaged;

    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject heart;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip Hit;
    [SerializeField] private AudioClip Shot;

    [Header("Stat")]
    public float MaxHealth;
    [SerializeField] private float health;
    public float atkSpeed = 1f;
    private float atkTimer = 0f;
    public float range = 1f;
    public float bulletSpeed = 1f;
    public int Pierce = 0;
    public float dmg = 1f;
    public State state;
    public float Health
    {
        get { return health; }
        set 
        { 
            health = Mathf.Clamp(value, 0, MaxHealth);
            HealthBarUpdate();
        }
    }

    private void Start()
    {
        Health = MaxHealth;
        rb = GetComponent<Rigidbody>();
        defMaterial = rbSprite.sprite;
        audioSource = GetComponent<AudioSource>();
        HealthBarUpdate();
        //print(this.GetType().Name);
    }

    public void Damage(float amount)
    {
        if (state == State.IFrame) return;
        state = State.IFrame;
        audioSource.PlayOneShot(Hit);
        Health -= amount;
        Invoke("IFramer",.75f);
        StartCoroutine(WhiteWash());
        if (Health <= 0)
        {
            Die();
        }
    }

    private void HealthBarUpdate()
    {
        int currentHearts = Mathf.CeilToInt(Health); // 현재 체력에 맞는 하트의 수
        int childCount = canvas.childCount; // 현재 캔버스에 있는 하트 아이콘의 수

        // 필요한 하트 수를 확인하고, 부족하면 추가하거나 초과하면 제거
        if (currentHearts < childCount)
        {
            // 제거해야 할 하트 수만큼 반복
            for (int i = 0; i < childCount - currentHearts; i++)
            {
                // 가장 마지막 하트부터 제거
                PoolManager.Release(canvas.GetChild(canvas.childCount - 1).gameObject);
            }
        }
        else if (currentHearts > childCount)
        {
            // 추가해야 할 하트 수만큼 반복
            for (int i = 0; i < currentHearts - childCount; i++)
            {
                // 하트 추가
                GameObject dick = PoolManager.Get(heart, canvas);
                dick.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    private void IFramer() 
    {
        if (Health <= 0) return;
        state = State.None;
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        state = State.Dead;
        StartCoroutine(Death());
    }
    private void Update()
    {
        Vector3 shootDirection = Vector3.zero;
        float moveX = Input.GetAxisRaw("HorizontalCum");
        float moveZ = Input.GetAxisRaw("VerticalCum");

        shootDirection = Vector3.ClampMagnitude(new Vector3(moveX, 0.0f, moveZ), 1f);
        atkTimer += Time.deltaTime;

        if (shootDirection != Vector3.zero && atkTimer >= .4f/atkSpeed && state != State.Dead)  // 방향키가 눌린 경우
        {
            atkTimer = 0f;
            Attack(shootDirection);
        }
    }

    private void Attack(Vector3 shootDirection)
    {
        audioSource.PlayOneShot(Shot);
        GameObject cum = PoolManager.Get(bullet, transform.position, Quaternion.identity);
        cum.GetComponent<Bullet>().lifetime = range;
        cum.GetComponent<Bullet>().Damage = dmg;
        cum.GetComponent<Rigidbody>().velocity = shootDirection * 8f * bulletSpeed;
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (state == State.IFrame) return;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        HitStopper.Instance.StopTime(0, .15f);
    }

    IEnumerator WhiteWash()
    {
        rbSprite.sprite = cumMaterial;
        GameObject fx = PoolManager.Get(damaged, transform.position,transform.rotation);
        fx.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(.2f);
        rbSprite.sprite = defMaterial;
        yield return new WaitForSeconds(1f);
        PoolManager.Release(fx);
    }

    IEnumerator Death()
    {
        if (rb != null)
        {
            rb.isKinematic = true; // Rigidbody를 Kinematic으로 설정하여 물리 영향을 받지 않게 함
            rb.detectCollisions = false; // 충돌 감지를 비활성화
        }
        Vector3 origin = transform.position;
        var time = new WaitForSeconds(.0001f);
        for (int i = 0; i < 100; i++)
        {
            transform.position = origin + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            yield return time;
        }
        rbSprite.enabled = false;
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }
}
