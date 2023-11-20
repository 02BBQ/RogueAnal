using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HolyCross : Enemy
{
    private float tick = 0;
    private GameObject playerBody;

    public UnityEvent OnAttack;

    private Animator animator;

    [SerializeField] private GameObject Bullet;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private Vector3[] directions = new Vector3[]
        {
            Vector3.forward,  // 전방
            Vector3.back,     // 후방
            Vector3.left,     // 좌측
            Vector3.right     // 우측
        };

    new private void OnEnable()
    {
        playerBody = Player.Instance.gameObject;
    }
    private void Update()
    {
        tick += Time.deltaTime;
        if (tick > 1f && playerBody != null)
        {
            tick = 0;
            animator.SetTrigger("Attack");
            foreach (Vector3 dir in directions)
            {
                GameObject bullet = PoolManager.Get(Bullet, transform.position, Quaternion.identity);
                bullet.GetComponent<EnemyWeak>().shooter = gameObject;
                bullet.GetComponent<Rigidbody>().velocity = dir * 5.5f; // 해당 방향으로 총알 발사
            }
        }
    }
}
