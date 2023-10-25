using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemySO : ScriptableObject
{
    public float Health = 100;

    public float AIUpdateInterval = .05f;

    public float Acceleration = 8;
    public float AngularSpeed = 120;

    public int AreaMask = 6;
    public int AvoidancePriority = 50;
    public float BaseOffset = 0;
    public float Height = 2f;
    public ObstacleAvoidanceType ObstacleAvoidanceType;
    public float Radius = .5f;
    public float Speed = 3f;
    public float StoppingDistance = .5f;

    public GameObject Prefab;
    [Range(0f, 100f)] public float Chance = 100f;

    public double _weight;
}
