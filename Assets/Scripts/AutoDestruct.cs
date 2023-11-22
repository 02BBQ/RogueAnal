using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public float LifeTime = 1f;
    private void OnEnable()
    {
        if(this.GetComponent<ParticleSystem>())
        { this.GetComponent<ParticleSystem>().Play(); }
        Invoke("Destruct",LifeTime);
    }
    private void Destruct()
    {
        PoolManager.Release(this);
    }
}
