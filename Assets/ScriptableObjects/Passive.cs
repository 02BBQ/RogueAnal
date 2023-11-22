using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Passive : MonoBehaviour
{
    public ItemSO itemSO;
    
    public void Active()
    {
        if(itemSO != null)
        {
            throw new NotImplementedException("ItemSO NOT ASSIGNED"); 
        }
        Player.Instance.Health += itemSO.healthRecovery;
        Player.Instance.MaxHealth += itemSO.maxHealthRecovery;
    }
}
