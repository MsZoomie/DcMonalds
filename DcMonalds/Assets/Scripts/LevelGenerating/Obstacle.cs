using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObstacleInstance
{
    public Obstacle obstaclePrefab;

    public bool spawnFrequently;
    public int spacing = 2;
    public Vector3 firstSpawnPosition;

    [HideInInspector]
    public Transform lastInstance;

    [Tooltip("The sum of all of the randomly spawning obstacle's spawn probability has to be 100 or less.")]
    [Range(0, 100)]
    public int spawnProbability = 50;
}


[Serializable]
[CreateAssetMenu]
public class Obstacle : ScriptableObject
{
    public GameObject prefab;
    public int tilesCovered = 1;
}