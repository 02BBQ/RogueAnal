using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text money;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        money.text = GameManager.Instance.Money.ToString();
    }
}
