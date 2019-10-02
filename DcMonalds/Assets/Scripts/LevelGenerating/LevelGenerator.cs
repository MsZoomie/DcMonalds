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

        public GameObject GetPrefab()
        {
            return prefab;
        }
    }

    [Serializable]
    private class Obstacle
    {
        public GameObject prefab;

        public bool spawnFrequently;
        public int spacing = 2;

        public Transform lastInstance;
        

        [Range(0, 100)]
        public int spawnProbability = 50;

    }


    [SerializeField]
    public int numberOfRows = 3;

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

        sidewalkPrefab = UnityEngine.Resources.Load<GameObject>("LanePrefabs/Sidewalk");
        grassPrefab = UnityEngine.Resources.Load<GameObject>("LanePrefabs/Grass");
        roadPrefab = UnityEngine.Resources.Load<GameObject>("LanePrefabs/Road");

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
        GameObject levelObstacles = new GameObject("Obstacles");
        levelObstacles.transform.SetParent(tempLevel.transform);

        for (int i = 0; i < numberOfRows; i++)
        {
            GameObject row = new GameObject("Row");
            row.transform.position += Vector3.forward * i;
            row.transform.SetParent(tempLevel.transform);


            for (int j = 0; j < lanes.Count; j++)
            {
                Vector3 tempPos = new Vector3(row.transform.position.x + j, row.transform.position.y, row.transform.position.z);
                GameObject tile = Instantiate(lanes[j].GetPrefab(), tempPos, Quaternion.identity, row.transform);
                
                // skip pathfinder if spawnFrequently is true
                // use pathfinder
                // if there is a path to the tile:
                // check that there is not more than lanes.count - 2 obstacles on the row
                // 

                if (lanes[j].obstacles.Count > 0)
                {
                    InstantiateObstacle(lanes[j].obstacles[0], tile.transform, levelObstacles.transform);
                }
            }
        }
    }


    /// <summary>
    /// Instantiates an obstacle.
    /// If spawnFrequently is true for the obstacle, it will spawn in even intervals according to the spacing value.
    /// If spawnFrequently is false for the obstacle, it will be spawn according to the spawnProbablility value.
    /// </summary>
    /// <param name="obstacle">The obstacle to be instatiated.</param>
    /// <param name="transformParent">Transform of the tile the obstacle will be placed upon.</param>
    /// <param name="parentObject">Transform of the game object in the scene which will be set as parent.</param>
    private void InstantiateObstacle(Obstacle obstacle, Transform transformParent, Transform parentObject)
    {
        if (obstacle.spawnFrequently)
        {
            if (obstacle.lastInstance != null)
            {
                float dist = Mathf.Abs(obstacle.lastInstance.position.z - transformParent.position.z);
                if (dist < obstacle.spacing)
                {
                    return;
                }
            }
            

            Vector3 tempPos = new Vector3(transformParent.transform.position.x - 0.5f, transformParent.transform.position.y + 1, transformParent.transform.position.z + 0.5f);

            GameObject newObstacle = Instantiate(obstacle.prefab, tempPos, Quaternion.identity, parentObject);
            newObstacle.name = obstacle.prefab.name;

            obstacle.lastInstance = newObstacle.transform;
        }
        else
        {
            float probability = obstacle.spawnProbability * 0.01f;
            float rand = UnityEngine.Random.value;

            if (rand <= probability)
            {
                Vector3 tempPos = new Vector3(transformParent.transform.position.x - 0.5f, transformParent.transform.position.y + 1, transformParent.transform.position.z + 0.5f);

                GameObject newObstacle = Instantiate(obstacle.prefab, tempPos, Quaternion.identity, parentObject);
                newObstacle.name = obstacle.prefab.name;
            }
        }
    }
}



public class Level : MonoBehaviour
{
   // SearchSpace searchSpace = new SearchSpace();

}