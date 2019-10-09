using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class LevelTheme : ScriptableObject
{
    public List<LaneInfo> lanes = new List<LaneInfo>();

    public List<Extra> extras = new List<Extra>();
}



[Serializable]
public class LaneInfo
{
    public GameObject laneTypePrefab;
    [Space]
    public List<ObstacleInstance> obstacles = new List<ObstacleInstance>();

    public GameObject GetPrefab()
    {
        return laneTypePrefab;
    }
}

[Serializable]
public class Extra
{
    public GameObject prefab;
    public bool repeatingX;
    public bool repeatingY;
    public Vector2 interval;
    public Vector3 startPos;
}

