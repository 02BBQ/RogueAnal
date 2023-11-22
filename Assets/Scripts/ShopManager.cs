using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ItemSO[] items;
    private float accumulatedWeights;

    private System.Random rand = new System.Random();

    private void Start()
    {
        CalculateWeights();
    }
    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (ItemSO item in items)
        {
            accumulatedWeights += item.chance;
            item.weight = accumulatedWeights;
        }
    }
    public ItemSO GetRandomEnemyIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < items.Length; i++)
            if (items[i].weight >= r)
                return items[i];

        return null;
    }
    private void OnEnable()
    {
        foreach (Transform go in transform)
        {
            go.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (Transform go in transform)
        {
            go.gameObject.SetActive(false);
        }
    }
}
