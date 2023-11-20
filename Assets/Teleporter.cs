using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleporter : Singleton<Teleporter>
{
    private Transform playerBody;
    public bool entered = false;

    public UnityEvent Enter;
    public UnityEvent Exit;
    public UnityEvent PressF;

    private void Start()
    {
        playerBody = Player.Instance.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entered = true;
            Enter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entered = false;
            Exit?.Invoke();
        }
    }
}
