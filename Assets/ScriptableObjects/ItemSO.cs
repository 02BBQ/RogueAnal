using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Config", menuName = "Shop/Item Config")]
public class ItemSO : ScriptableObject
{
    public string Name;
    public string Desc;
    public GameObject Prefab;

    public float weight = 0;
    public float chance = 0;
    public int price = 0;

    public float healthRecovery = 0;
    public float maxHealthRecovery = 0;

    public AudioClip soundEffect;
}
