using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Starr : Enemy
{
    private float tick = 0;
    private float attackTick;
    private GameObject playerBody;
    private bool tracking = true;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private Collider hitbox;

    public UnityEvent OnAttack;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

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
            // when he get close to player, he charges
            if (distanceToPlayer <= 3.5f)
            {
                if (attackTick >= 2f)
                {
                    hitbox.enabled = true;
                    animator.SetTrigger("Charge");
                    Invoke("Dash", .2f);
                    attackTick = 0;
                    OnAttack?.Invoke();
                }
                agent.Stop();
            }
            else
            {
                if (agent.isStopped) agent.Resume();
                agent.SetDestination(playerBody.transform.position);
            }
            tick = 0;
        }
    }
    

    public void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<IDamageable>() != null)
        {
            Vector3 knockbackDirection = playerBody.transform.position - transform.position;
            Player.Instance.ApplyKnockback(knockbackDirection, 75f);
            other.GetComponent<IDamageable>().Damage(1f);
            
            StartCoroutine(cooldown());
        }
    }

    private void Dash()
    {
        rb.AddForce(transform.forward * 17f, ForceMode.Impulse);
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(.3f);
        hitbox.enabled = false;
    }
}
