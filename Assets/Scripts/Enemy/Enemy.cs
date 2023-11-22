using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    public NavMeshAgent agent;
    public EnemySO enemySO;

    public Material defaultMaterial;
    public Material cum;

    private float MaxHealth;

    public float Health;
    //{
    //    get => Health;
    //    set => Mathf.Clamp(value, 0, MaxHealth);
    //}
    [HideInInspector] public double _weight;
    public float Chance = 100f;
    [SerializeField] private GameObject deathFX;

    private void Awake()
    {
        agent.acceleration = enemySO.Acceleration;
        agent.angularSpeed = enemySO.AngularSpeed;
        agent.areaMask = enemySO.AreaMask;
        agent.avoidancePriority = enemySO.AvoidancePriority;
        agent.baseOffset = enemySO.BaseOffset;
        agent.height = enemySO.Height;
        agent.obstacleAvoidanceType = enemySO.ObstacleAvoidanceType;
        agent.radius = enemySO.Radius;
        agent.speed = enemySO.Speed;
        agent.stoppingDistance = enemySO.StoppingDistance;

        MaxHealth = enemySO.Health;
        Health = MaxHealth;
        SetUpAgentFromConfig();
    }

    public virtual void OnEnable()
    {
        SetUpAgentFromConfig();
    }

    public void OnDisable()
    {
        //agent.enabled = false;
    }

    public virtual void SetUpAgentFromConfig()
    {
        agent.speed = enemySO.Speed;

        MaxHealth = enemySO.Health;
        Health = MaxHealth;

        if (GameManager.BuffVal > 1)
        {
            agent.speed = enemySO.Speed * (1 + GameManager.BuffVal / 3);

            MaxHealth *= GameManager.BuffVal;
            Health = MaxHealth;
        }


        _weight = enemySO._weight;

        Chance = enemySO.Chance;

    }

    public virtual void Damage(float amount)
    {
        Health -= amount;

        if (GetComponentInChildren<SpriteRenderer>())
        {
            GameManager.Instance.whiteWashing(gameObject);
        }

        if (Health<=0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(gameObject.GetComponent<LootDrop>()) gameObject.GetComponent<LootDrop>().Drop();
        if (deathFX != null) PoolManager.Get(deathFX, transform.position, Quaternion.identity);;
        GameManager.Instance.Killed++;
        PoolManager.Release(gameObject);
    }
}
