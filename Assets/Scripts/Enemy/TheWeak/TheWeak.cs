using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheWeak : Enemy
{
    private float tick = 0;
    private GameObject playerBody;

    private void Start()
    {
        playerBody = Player.Instance.gameObject;
    }

    [System.Obsolete]
    private void Update()
    {
        tick += Time.deltaTime;
        if (tick > .05f && playerBody != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerBody.transform.position);
            // 플레이어와 일정 거리 안에 들어가면 뒤로 물러남
            if (distanceToPlayer <= 7f)
            {
                Vector3 moveDirection = (transform.position - playerBody.transform.position).normalized;
                Vector3 newPosition = transform.position + moveDirection * (7f - distanceToPlayer);
                agent.SetDestination(newPosition);
            }
            else if (distanceToPlayer >= 30f)
            {
                agent.SetDestination(playerBody.transform.position);
            }
            else
            {
                agent.Stop();
            }
        }
    }
}
