using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    public int numberOfRows = 3;

    [SerializeField]
    private List<LaneInfo> lanes;


    // lane prefabs
    private GameObject sidewalkPrefab;
    private GameObject grassPrefab;
    private GameObject roadPrefab;

    private SearchSpace searchSpace;
    public Pathfinder pathfinder;

    private void Awake()
    {
        if (pathfinder == null)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
        }

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
        if (pathfinder == null)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
        }

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


        GameObject level = new GameObject("Level");
        level.AddComponent<Level>();
        searchSpace = level.AddComponent<SearchSpace>();

        GameObject rows = new GameObject("Rows");
        rows.transform.SetParent(level.transform);

        GameObject levelObstacles = new GameObject("Obstacles");
        levelObstacles.transform.SetParent(level.transform);
        pathfinder.ResetPathfinder();
        pathfinder.searchSpace = searchSpace;


        for (int rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
        {
            //Create new row
            GameObject row = new GameObject("Row");
            row.transform.position += Vector3.forward * rowIndex;
            row.transform.SetParent(rows.transform);

            //remove latest checked row from search space
            searchSpace.tiles.RemoveAll(x => searchSpace.startRow.Contains(x));
            searchSpace.startRow = searchSpace.endRow;
            searchSpace.endRow.Clear();
            

            for (int laneIndex = 0; laneIndex < lanes.Count; laneIndex++)
            {
                // Add a tile to row
                Vector3 tempPos = new Vector3(row.transform.position.x + laneIndex, row.transform.position.y, row.transform.position.z);
                GameObject node = Instantiate(lanes[laneIndex].GetPrefab(), tempPos, Quaternion.identity, row.transform);
                searchSpace.AddTile(node);
                searchSpace.endNode = node;

                // add the tile to next row to be examined
                Tile tile = node.GetComponent<Tile>();
                tile.AddToEndRow();
                tile.UpdateTile();
                


                //if there's already an obstacle here, we don't want to add a new one
                if (tile.hasObstacle)
                {
                    goto ObstacleAdded;
                }

                //do pathfinder
                bool placeObstacle = true;
                if (rowIndex < 0)
                {
                    placeObstacle = UsePathfinder(node);
                }

                // skip pathfinder if spawnFrequently is true
                // use pathfinder
                // if there is a path to the tile:
                // check that there is not more than lanes.count - 2 obstacles on the row
                // 
                if (lanes[laneIndex].obstacles.Count > 0 && placeObstacle)
                {
                    InstantiateObstacle(lanes[laneIndex].obstacles, node.transform, levelObstacles.transform);
                }

                ObstacleAdded: { }
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
    private void InstantiateObstacle(List<Obstacle> obstacles, Transform transformParent, Transform parentObject)
    {
        Obstacle obstacle = ChooseObstacle(obstacles, transformParent);
        Vector3 tempPos = new Vector3(transformParent.transform.position.x, transformParent.transform.position.y + 1, transformParent.transform.position.z);

        if (obstacle == null)
        {
            return;
        }
        
        GameObject newObstacle = Instantiate(obstacle.prefab, tempPos, Quaternion.identity, parentObject);
        newObstacle.name = obstacle.prefab.name;

        if (obstacle.spawnFrequently)
        {
            obstacle.lastInstance = newObstacle.transform;
        }
    }

    /// <summary>
    /// Choose which obstacle to be spawned.
    /// </summary>
    /// <param name="obstacles">List of Obstacles available for lane.</param>
    /// <param name="transformParent">Transform of the tile upon which obstacle will be spawned.</param>
    /// <returns>Obstacle to be spawned. Might return null if</returns>
    private Obstacle ChooseObstacle(List<Obstacle> obstacles, Transform transformParent)
    {
        //look for all the frequently spawning obstacles, they have priority
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i].spawnFrequently)
            {
                if (obstacles[i].lastInstance == null)
                {
                    float dist = Mathf.Abs(obstacles[i].firstSpawnPosition.z - transformParent.position.z);
                    if (Mathf.Approximately(dist, 0))
                    {
                        return obstacles[i];
                    }
                }
                else
                {
                    float dist = Mathf.Abs(obstacles[i].lastInstance.position.z - transformParent.position.z);
                    if (dist >= obstacles[i].spacing)
                    {
                        return obstacles[i];
                    }
                }
            }
        }

        // if there wasn't any frequently spawning obstacles matching this position, we look to the randomly spawning obstacles
        // Roulette Wheel Selection
        for (int i = 0; i < obstacles.Count; i++)
        {
            float preProbability = 0.0f;
            float rand = UnityEngine.Random.value;

            if (!obstacles[i].spawnFrequently)
            {
                float probability = obstacles[i].spawnProbability * 0.01f;
                preProbability += probability;

                if (rand <= preProbability)
                {
                    return obstacles[i];
                }
            }
            if (preProbability > 1)
            {
                Debug.Log("The sum of all of the randomly spawning obstacle's spawn probability on this lane has to be 100 or less.");
            }
        }

        return null;
    }


    private bool UsePathfinder(GameObject node)
    {
        return pathfinder.CheckAllPaths(node);
    }
}



public class Level : MonoBehaviour
{
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

}