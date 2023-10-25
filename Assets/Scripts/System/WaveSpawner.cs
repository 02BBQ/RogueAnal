using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
//public class Enemy
//{
//    // for debug :
//    public string Name;

//    public GameObject Prefab;
//    [Range(0f, 100f)] public float Chance = 100f;

//    [HideInInspector] public double _weight;
//}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawner;
    private System.Random rand = new System.Random();
    private double accumulatedWeights;
    private int maxAttempts = 10;

    private float spawnAreaRadius = 50f; // ������ ������ ������
    private float playerSafeRadius = 5f; // �÷��̾� �ֺ��� ������ ������

    private Transform body;

    [SerializeField] private EnemySO[] enemies;

    private int enemyCount = 2;

    private void Awake()
    {
        CalculateWeights();
    }
    private void Start()
    {
        //Enemy randomEnemy = enemies[GetRandomEnemyIndex()];
        //StartWave();
        body = Player.Instance.transform;
        StartWave();
    }
            
    private void StartWave()
    {
        StartCoroutine(DoWave());
    }

    public Vector3 SpawnEnemyPosition()
    {
        Vector3 spawnPosition;
        int currentAttempt = 0;

        do
        {
            // ������ ���� ������ ������ ��ġ ����
            float x = Random.Range(-spawnAreaRadius, spawnAreaRadius);
            float z = Random.Range(-spawnAreaRadius, spawnAreaRadius);
            spawnPosition = new Vector3(x, 0, z + 25); // y ���� 0�̳� �ٸ� ������ ������ ������ �� �ֽ��ϴ�.

            currentAttempt++;
            if (currentAttempt > maxAttempts) break; // �ִ� �õ� Ƚ���� �ʰ��ϸ� ������ �������ɴϴ�.
        } while (Vector3.Distance(spawnPosition, body.position) < playerSafeRadius); // �÷��̾� �ֺ� �ݰ� �ȿ� �ִ��� Ȯ��
        return spawnPosition;
    }

    IEnumerator DoWave()
    {
        enemyCount += UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < enemyCount; i++)
        {
            EnemySO randomEnemy = enemies[GetRandomEnemyIndex()];
            var enemyGO = PoolManager.Get(randomEnemy.Prefab, SpawnEnemyPosition(), Quaternion.identity,transform);
        }
        yield return null;
    }

    private void OnTransformChildrenChanged()
    {
        if (transform.childCount <= 0)
        {
            StartCoroutine(DoWave());
        }
    }

    private int GetRandomEnemyIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < enemies.Length; i++)
            if (enemies[i]._weight >= r)
                return i;

        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (EnemySO enemy in enemies)
        {
            accumulatedWeights += enemy.Chance;
            enemy._weight = accumulatedWeights;
        }
    }
}