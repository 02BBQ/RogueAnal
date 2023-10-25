using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weakest : Enemy
{
    private float tick = 0;
    private GameObject playerBody;

    private void Start()
    {
        playerBody = Player.Instance.gameObject;
    }
    private void Update()
    {
        tick += Time.deltaTime;
        if (tick > .05f && playerBody != null)
        {
            agent.SetDestination(playerBody.transform.position);
        }
    }

    public void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().Damage(1f);
            Vector3 knockbackDirection = playerBody.transform.position - transform.position;
            float knockbackForce = 50;  // Set this to whatever value you find suitable
            Player.Instance.ApplyKnockback(knockbackDirection, knockbackForce);
        }
    }
}
