using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    public NavMeshAgent agent;
    public EnemySO enemySO;

    public float MaxHealth;

    public float Health;
    //{
    //    get => Health;
    //    set => Mathf.Clamp(value, 0, MaxHealth);
    //}
    [HideInInspector] public double _weight;
    public float Chance = 100f;

    public virtual void OnEnable()
    {
        SetUpAgentFromConfig();
    }
    public void OnDisable()
    {
        agent.enabled = false;
    }

    public virtual void SetUpAgentFromConfig()
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

        _weight = enemySO._weight;

        Chance = enemySO.Chance;
    }

    public virtual void Damage(float amount)
    {
        Health -= amount;
        if (Health<=0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        PoolManager.Release(gameObject);
    }
}
