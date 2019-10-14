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
    [Tooltip("Has to be at least 1. \nThe total ratio of the lane types has to be at least 1")]
    public LaneType[] laneTypes;
    [Space]
    public List<ObstacleInstance> obstacles = new List<ObstacleInstance>();

    public GameObject GetPrefab()
    {
        return GetLaneType();
    }




    private GameObject GetLaneType()
    {
        float ratioSum = 0;
        float preProb = 0;
        float rand = UnityEngine.Random.value;

        for (int i = 0; i < laneTypes.Length; i++)
        {
            ratioSum += laneTypes[i].ratio;
        }

        for (int i = 0; i < laneTypes.Length; i++)
        {
            preProb += laneTypes[i].ratio / ratioSum;

            if (rand <= preProb)
            {
                return laneTypes[i].prefab;
            }
        }
        return null;
    }


    [Serializable]
    public struct LaneType
    {
        public GameObject prefab;
        public int ratio;
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

