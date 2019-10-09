using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTheme : MonoBehaviour
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
}



[Serializable]
public class LaneInfo
{
    public enum LaneType
    { Sidewalk, Grass, Road }

    public LaneType type;
    public List<Obstacle> obstacles = new List<Obstacle>();

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

[Serializable]
public class Obstacle
{
    public GameObject prefab;

    public bool spawnFrequently;
    public int spacing = 2;
    public Vector3 firstSpawnPosition;

    //[HideInInspector]
    public Transform lastInstance;

    [Tooltip("The sum of all of the randomly spawning obstacle's spawn probability has to be 100 or less.")]
    [Range(0, 100)]
    public int spawnProbability = 50;


    public int tilesCovered = 1;
}