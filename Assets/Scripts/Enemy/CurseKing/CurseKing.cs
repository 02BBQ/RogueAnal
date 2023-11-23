using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class CurseKing : Enemy
{
    private float tick = 0;
    private float attackTick;
    private GameObject playerBody;
    private bool tracking = true;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Collider hitbox;

    public UnityEvent OnAttack;

    new private void OnEnable()
    {
        playerBody = Player.Instance.gameObject;
    }

    [System.Obsolete]
    private void Update()
    {
        if (tracking && playerBody != null)
        {
            Quaternion quaternion = Quaternion.LookRotation(new Vector3(playerBody.transform.position.x, base.transform.position.y, playerBody.transform.position.z) - transform.position);
            base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * Mathf.Clamp(Quaternion.Angle(base.transform.rotation, quaternion) * 2f, 15f, 90f));
        }
        tick += Time.deltaTime;
        attackTick += Time.deltaTime;
        if (tick > .05f && playerBody != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerBody.transform.position);
            // when too close to player, enemy do slickback to have distance
            if (distanceToPlayer <= 5f)
            {
                agent.Resume();
                agent.SetDestination(playerBody.transform.position);
            }
            else
            {
                if (attackTick >= 1.5f)
                {
                    GameObject bullet = PoolManager.Get(Bullet, transform.position, transform.rotation);
                    bullet.GetComponent<EnemyWeak>().shooter = gameObject;
                    bullet.GetComponent<Rigidbody>().velocity = transform.forward * 15f;
                    attackTick = 0;
                }
                agent.Stop();
            }
            tick = 0;
        }
    }

    public void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<IDamageable>() != null)
        {
            Vector3 knockbackDirection = playerBody.transform.position - transform.position;
            float knockbackForce = 10f;
            Player.Instance.ApplyKnockback(knockbackDirection, knockbackForce);
            other.GetComponent<IDamageable>().Damage(.8f);
            hitbox.enabled = false;
            OnAttack?.Invoke();
            StartCoroutine(cooldown());
        }
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(.8f);
        hitbox.enabled = true;
    }
}
