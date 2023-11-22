using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject deathFX;
    [SerializeField] private GameObject hitFX;
    public float lifetime = 1f;

    public float Damage = 1f;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7 && c.GetComponent<IDamageable>() != null) 
        {
            if (c.CompareTag("Player")) return;
            c.GetComponent<IDamageable>().Damage(Damage);
            PhysicsManager.Instance.ApplyKnockback(c.GetComponent<Rigidbody>(), (new Vector3(c.transform.position.x, transform.position.y, c.transform.position.z) - transform.position).normalized, 7.5f) ;
            PoolManager.Get(hitFX, transform.position, Quaternion.identity);
            Release();
        }
    }

    private void OnEnable()
    {
        lifetime = Player.Instance.range;
        StopAllCoroutines();
        StartCoroutine("DestructAuto", lifetime);
    }

    IEnumerator DestructAuto(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Release();
    }
    
    private void Release()
    {
        StopAllCoroutines();
        PoolManager.Get(deathFX, transform.position, Quaternion.identity);
        PoolManager.Release(gameObject);
    }
}
