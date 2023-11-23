using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopStand : MonoBehaviour
{
    private ShopManager shopManager;
    public GameObject curr;
    public ItemSO currSO;

    public RectTransform movingObject;
    public TMP_Text Name;
    public TMP_Text desc;

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
        movingObject.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        movingObject.gameObject.SetActive(false);
        if (curr != null) 
        {
            try
            {
                Destroy(curr);
            }
            catch { }
        }
    }

    private void OnMouseEnter()
    {
        movingObject.gameObject.SetActive(true);
        Name.text = currSO.Name;
        desc.text = currSO.Desc;
    }
    private void OnMouseOver()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        movingObject.position = pos;
    }
    private void OnMouseExit()
    {
        movingObject.gameObject.SetActive(false);
    }
}
