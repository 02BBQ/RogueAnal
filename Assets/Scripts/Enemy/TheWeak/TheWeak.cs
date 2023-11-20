using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheWeak : Enemy
{
    private float tick = 0;
    private float attackTick;
    private GameObject playerBody;
    private bool tracking = true;
    [SerializeField] private GameObject Bullet;

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
            if (distanceToPlayer <= 2f)
            {
                Vector3 moveDirection = (transform.position - playerBody.transform.position).normalized;
                Vector3 newPosition = transform.position + moveDirection * (7f - distanceToPlayer);
                agent.SetDestination(newPosition);
            }
            else if (distanceToPlayer >= 8f)
            {
                agent.Resume();
                agent.SetDestination(playerBody.transform.position);
            }
            else
            {
                if (attackTick >= 1f)
                {
                    GameObject bullet = PoolManager.Get(Bullet, transform.position, transform.rotation);
                    bullet.GetComponent<EnemyWeak>().shooter = gameObject;
                    bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
                    attackTick = 0;
                }
                agent.Stop();
            }
            tick = 0;
        }
    }
}
