using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObstacleInstance
{
    [Tooltip("You need to choose an obstacle from the prefab folder.")]
    public Obstacle obstaclePrefab;

    public bool spawnFrequently;
    [Header("If Spawn Frequently is true")]
    [Tooltip("Type int. \nOnly relevant if spawnFrequently is true.")]
    public int spacing = 2;
    [Tooltip("Only relevant if spawnFrequently is true.")]
    public Vector3 firstSpawnPosition;

    [HideInInspector]
    public Transform lastInstance;

    [Header("If Spawn Frequently is false")]
    [Tooltip("Type int. \nThe sum of all of the randomly spawning obstacle's spawn probability has to be 100 or less. \nOnly relevant if spawnFrequently is false.")]
    [Range(0, 100)]
    public int spawnProbability = 50;
}


[Serializable]
[CreateAssetMenu]
public class Obstacle : ScriptableObject
{
    [Tooltip("The game object prefab for this obstacle.")]
    public GameObject prefab;
    [Tooltip("The amount of tiles covered in direction of lanes.")]
    public int tilesCovered = 1;
}