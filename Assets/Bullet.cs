using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward*20f;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 7 && c.GetComponent<IDamageable>() != null) 
        {
            if (c.CompareTag("Player")) return;
            c.GetComponent<IDamageable>().Damage(15f);
        }
    }
}
