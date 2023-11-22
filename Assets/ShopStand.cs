using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStand : MonoBehaviour
{
    private ShopManager shopManager;
    public GameObject curr;
    public ItemSO currSO;

    private void Awake()
    {
        shopManager = transform.parent.GetComponent<ShopManager>();
    }

    private void OnEnable()
    {
        currSO = shopManager.GetRandomEnemyIndex();
        curr = PoolManager.Get(currSO.Prefab,transform.position + Vector3.up, Quaternion.identity);
        curr.transform.SetParent(transform,true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (currSO.price > GameManager.Instance.Money) return;
        GameManager.Instance.Money -= currSO.price;
        if (curr.GetComponent<Passive>())
        {
            curr.GetComponent<Passive>().Active();
        }
        PoolManager.Release(curr);
    }
    private void OnDisable()
    {
        if (curr != null) 
        {
            try
            {
                PoolManager.Release(curr);
            }
            catch { }
        }
    }
}
