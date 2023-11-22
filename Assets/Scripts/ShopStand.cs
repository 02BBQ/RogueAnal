using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopStand : MonoBehaviour
{
    private ShopManager shopManager;
    public GameObject curr;
    public ItemSO currSO;

    [SerializeField] GameObject buy;

    private void Awake()
    {
        shopManager = transform.parent.GetComponent<ShopManager>();
        currSO = shopManager.GetRandomEnemyIndex();
    }

    private void OnEnable()
    {
        currSO = shopManager.GetRandomEnemyIndex();
        curr = Instantiate(currSO.Prefab,transform.position + Vector3.up, Quaternion.identity);
        curr.transform.SetParent(transform,true);
        transform.Find("Canvas/Text").GetComponent<TextMeshProUGUI>().text = $"${currSO.price}";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || curr == null) return;
        if (currSO.price > GameManager.Instance.Money) return;
        GameManager.Instance.Money -= currSO.price;
        PoolManager.Get(buy, Player.Instance.transform.position, Quaternion.identity);
        if (curr.GetComponent<Passive>())
        {
            curr.GetComponent<Passive>().Active();
        }
        PoolManager.Release(curr);
        HitStopper.Instance.StopTime(0, .25f);
        CinemachineShake.Instance.ShakeCamera(15, .7f);
    }
    private void OnDisable()
    {
        if (curr != null) 
        {
            try
            {
                Destroy(curr);
            }
            catch { }
        }
    }
}
