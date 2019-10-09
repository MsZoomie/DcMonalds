using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class LevelTheme : ScriptableObject
{
    public List<LaneInfo> lanes = new List<LaneInfo>();

    private GameObject sidewalkPrefab;
    private GameObject grassPrefab;
    private GameObject roadPrefab;

    private void Awake()
    {
        sidewalkPrefab = Resources.Load<GameObject>("LanePrefabs/Sidewalk");
        grassPrefab = Resources.Load<GameObject>("LanePrefabs/Grass");
        roadPrefab = Resources.Load<GameObject>("LanePrefabs/Road");


        for (int i = 0; i < lanes.Count; i++)
        {
            switch (lanes[i].type)
            {
                case LaneInfo.LaneType.Sidewalk:
                    lanes[i].SetPrefab(sidewalkPrefab);
                    break;
                case LaneInfo.LaneType.Grass:
                    lanes[i].SetPrefab(grassPrefab);
                    break;
                case LaneInfo.LaneType.Road:
                    lanes[i].SetPrefab(roadPrefab);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnValidate()
    {
        sidewalkPrefab = Resources.Load<GameObject>("LanePrefabs/Sidewalk");
        grassPrefab = Resources.Load<GameObject>("LanePrefabs/Grass");
        roadPrefab = Resources.Load<GameObject>("LanePrefabs/Road");


        for (int i = 0; i < lanes.Count; i++)
        {
            switch (lanes[i].type)
            {
                case LaneInfo.LaneType.Sidewalk:
                    lanes[i].SetPrefab(sidewalkPrefab);
                    break;
                case LaneInfo.LaneType.Grass:
                    lanes[i].SetPrefab(grassPrefab);
                    break;
                case LaneInfo.LaneType.Road:
                    lanes[i].SetPrefab(roadPrefab);
                    break;
                default:
                    break;
            }
        }
    }

}



[Serializable]
public class LaneInfo
{
    

    public enum LaneType
    { Sidewalk, Grass, Road }

    public LaneType type;
    public List<ObstacleInstance> obstacles = new List<ObstacleInstance>();

    private GameObject prefab;

    public void SetPrefab(GameObject prefabObject)
    {
        prefab = prefabObject;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }
}

