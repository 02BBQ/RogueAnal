using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    public ItemSO itemSO;
    
    public void Active()
    {
        Player.Instance.Health += itemSO.healthRecovery;
        Player.Instance.MaxHealth += itemSO.maxHealthRecovery;
    }
}
