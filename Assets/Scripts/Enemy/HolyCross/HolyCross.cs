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
            Vector3.forward,  // ����
            Vector3.back,     // �Ĺ�
            Vector3.left,     // ����
            Vector3.right     // ����
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
                bullet.GetComponent<Rigidbody>().velocity = dir * 5.5f; // �ش� �������� �Ѿ� �߻�
            }
        }
    }
}
