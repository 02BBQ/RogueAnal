using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeak : MonoBehaviour
{
    public float lifetime = 2f;
    public GameObject shooter;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7 && c.GetComponent<IDamageable>() != null) 
        {
            if (c.gameObject == shooter && shooter != null) return;
            c.GetComponent<IDamageable>().Damage(2f);
            PhysicsManager.Instance.ApplyKnockback(c.GetComponent<Rigidbody>(), transform.forward, 3.5f) ;
            PoolManager.Release(gameObject);
            CancelInvoke("DestructAuto");
        }
    }

    private void OnEnable()
    {
        lifetime = Player.Instance.range;
        Invoke("DestructAuto", lifetime);
    }

    public void DestructAuto()
    {
        PoolManager.Release(gameObject);
    }
}
