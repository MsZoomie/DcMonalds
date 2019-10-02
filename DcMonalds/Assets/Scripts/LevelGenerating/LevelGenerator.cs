using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    [Serializable]
    private class LaneInfo
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

    }

    [Serializable]
    private class Obstacle
    {
        public GameObject prefab;
        [Range(0.0f, 100.0f)]
        public float frequency = 50f;
        [Range(0f, 100f)]
        public float randomnessDegree = 50f;
    }


    [SerializeField]
    public int numberOfRows = 1;

    [SerializeField]
    private List<LaneInfo> lanes;


    // lane prefabs
    private GameObject sidewalkPrefab;
    private GameObject grassPrefab;
    private GameObject roadPrefab;



    private void Awake()
    {

        if (lanes.Count <= 0)
        {
            for (int i = 0; i < 6; i++)
            {
                lanes.Add(new LaneInfo());
            }
        }

        sidewalkPrefab = UnityEngine.Resources.Load<GameObject>("LanePrefabs/Sidewalk.prefab");
        grassPrefab = UnityEngine.Resources.Load<GameObject>("LanePrefabs/Grass.prefab");
        roadPrefab = UnityEngine.Resources.Load<GameObject>("LanePrefabs/Road.prefab");
    }

    private void OnValidate()
    {
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


    public void GenerateLevel()
    {
        Level[] temp = FindObjectsOfType(typeof(Level)) as Level[];

        if (temp.Length > 0)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                DestroyImmediate(temp[i].gameObject);
            }
        }

        GameObject tempLevel = new GameObject("Level");
        tempLevel.AddComponent<Level>();

    }


}



public class Level : MonoBehaviour
{

}