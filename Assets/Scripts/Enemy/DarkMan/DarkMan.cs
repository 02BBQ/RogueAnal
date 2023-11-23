using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DarkMan : Enemy
{
    private float tick = 0;
    private GameObject playerBody;
    [SerializeField] private Collider hitbox;

    public UnityEvent OnAttack;

    new private void OnEnable()
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
            Vector3 knockbackDirection = playerBody.transform.position - transform.position;
            float knockbackForce = 10f;
            Player.Instance.ApplyKnockback(knockbackDirection, knockbackForce);
            other.GetComponent<IDamageable>().Damage(.75f);
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
