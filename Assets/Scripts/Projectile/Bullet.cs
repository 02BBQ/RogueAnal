using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject deathFX;
    public float lifetime = 1f;

    public float Damage = 1f;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7 && c.GetComponent<IDamageable>() != null) 
        {
            if (c.CompareTag("Player")) return;
            c.GetComponent<IDamageable>().Damage(Damage);
            PhysicsManager.Instance.ApplyKnockback(c.GetComponent<Rigidbody>(), (new Vector3(c.transform.position.x, transform.position.y, c.transform.position.z) - transform.position).normalized, 7.5f) ;
            PoolManager.Release(gameObject);
            Release();
        }
    }

    private void OnEnable()
    {
        lifetime = Player.Instance.range;
        CancelInvoke("DestructAuto");
        Invoke("DestructAuto", lifetime);
    }

    public void DestructAuto()
    {
        Release();
    }
    
    private void Release()
    {
        CancelInvoke("DestructAuto");
        PoolManager.Get(deathFX, transform.position, Quaternion.identity);
        PoolManager.Release(gameObject);
    }
}
