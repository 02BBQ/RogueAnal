using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public int MaxMoney = 5;
    public GameObject moneyPrefab;

    public void Drop()
    {
        for (int i = 0; i < Random.Range(0, MaxMoney + 1); i++)
        {
            var money = PoolManager.Get(moneyPrefab, transform.position, transform.rotation);
            money.GetComponent<Rigidbody>().velocity = transform.forward * 20;
        }
    }
}

