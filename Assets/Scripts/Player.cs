using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>, IDamageable
{
    [SerializeField] private GameObject bullet;
    private Rigidbody rb;
    public float MaxHealth;
    [SerializeField] private float health;
    private enum State
    {
        None,
        IFrame,
    }
    private State state;
    public float Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, MaxHealth); }
    }

    private void Start()
    {
        Health = MaxHealth;
        rb = GetComponent<Rigidbody>();
    }

    public void Damage(float amount)
    {
        if (state == State.IFrame) return;
        state = State.IFrame;
        Health -= amount;
        Invoke("IFramer",.75f);
        if (Health <= 0)
        {
            Die();
        }
    }

    private void IFramer() 
    {
        state = State.None;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PoolManager.Get(bullet, transform.position, Quaternion.identity);
        }
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (state == State.IFrame) return;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
