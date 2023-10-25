using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    private Rigidbody rb;
    public UnityEvent<Collider> trigger;

    private void Start()
    {
         rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        trigger?.Invoke(other);
    }
}
