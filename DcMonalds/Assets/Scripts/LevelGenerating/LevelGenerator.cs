using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    public int numberOfRows = 3;
    private int rowToStartPathfinderFrom = 2;
    private int numberOfLanes;

    /*
    [SerializeField]
    private List<LaneInfo> lanes = new List<LaneInfo>();
    */

    public LevelTheme theme;
    

    public Pathfinder pathfinder;

    
    private GameObject levelObstacles;

    private SearchSpace searchSpace;

    [HideInInspector]
    public int tilesChecked = 0;

    private void Awake()
    {
        if (pathfinder == null)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
        }

        if (theme == null)
        {
            theme = Resources.Load<LevelTheme>("LevelThemes/DefaultTheme");
        }

        numberOfLanes = theme.lanes.Count;
    }

    private void OnValidate()
    {
        if (pathfinder == null)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
        }
        if (theme == null)
        {
            theme = Resources.Load<LevelTheme>("LevelThemes/DefaultTheme");
        }

        
        numberOfLanes = theme.lanes.Count;
    }


    public void GenerateLevel()
    {
        Level[] levelToDestroy = FindObjectsOfType(typeof(Level)) as Level[];
  
        if (levelToDestroy.Length > 0)
        {
            for (int i = 0; i < levelToDestroy.Length; i++)
            {
                DestroyImmediate(levelToDestroy[i].gameObject);
            }
        }


        GameObject level = new GameObject("Level");
        level.AddComponent<Level>();
        searchSpace = level.AddComponent<SearchSpace>();

        GameObject rows = new GameObject("Rows");
        rows.transform.SetParent(level.transform);

        levelObstacles = new GameObject("Obstacles");
        levelObstacles.transform.SetParent(level.transform);


        if (numberOfLanes <= 0 || numberOfRows <= 0)
            return;

        //start adding rows
        for (int rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
        {
            //Create new row
            GameObject row = new GameObject("Row");
            row.transform.position += Vector3.forward * rowIndex;
            row.transform.SetParent(rows.transform);


            //prepare pathfinding for this row
            searchSpace.startRow.Clear();
            searchSpace.UpdateTiles();
            if (rowIndex >= rowToStartPathfinderFrom)
            {
                for (int i = 0; i < numberOfLanes; i++)
                {
                    int x = ((rowIndex - rowToStartPathfinderFrom) * numberOfLanes) + i;

                    //Debug.Log("Row " + rowIndex + ": " + x);
                    searchSpace.startRow.Add(searchSpace.tiles[x]);
                }
            }



            //start adding tiles
            for (int laneIndex = 0; laneIndex < numberOfLanes; laneIndex++)
            {
                // Add a tile to row
                Vector3 tempPos = new Vector3(row.transform.position.x + laneIndex, row.transform.position.y, row.transform.position.z);
                GameObject currentTile = Instantiate(theme.lanes[laneIndex].GetPrefab(), tempPos, Quaternion.identity, row.transform);
                searchSpace.endNode = currentTile;

                Tile currentNode = currentTile.GetComponent<Tile>();
                searchSpace.AddTile(currentNode);

                tilesChecked = 0;
                currentNode.CheckForObstacle(this);


                //if there's already an obstacle here, we don't want to add a new one
                if (currentNode.hasObstacle)
                {
                    goto ObstacleAdded;
                }


                //do pathfinder
                bool placeObstacle = true;
                if (rowIndex > rowToStartPathfinderFrom - 1)
                {
                    placeObstacle = UsePathfinder(currentNode);
                }
                

                if (placeObstacle)
                {
                    if (theme.lanes[laneIndex].obstacles.Count > 0 && placeObstacle)
                    {
                        currentNode.obstacle = ChooseObstacle(theme.lanes[laneIndex].obstacles, currentTile.transform);
                        if (currentNode.obstacle != null)
                            currentNode.hasObstacle = true;
                    }
                }
            ObstacleAdded: { }
            }
        }
    }


    public void ChooseLevel()
    {
        bool validLevel = false;

        while (!validLevel)
        {


            GenerateLevel();
            searchSpace.UpdateSearchSpace();


            for (int i = 0; i <numberOfLanes; i++)
            {
                searchSpace.startRow.Add(searchSpace.tiles[i]);
            }

            for (int i = numberOfRows * numberOfLanes - numberOfLanes; i < numberOfRows * numberOfLanes; i++)
            {
                bool found = UsePathfinderOnce(searchSpace.tiles[i]);

                if (found)
                {
                    validLevel = true;
                    break;
                }

            }


           // Debug.Log("The level is possible: " + validLevel);
        }



        if (validLevel)
        {
            for (int i = 0; i < searchSpace.tiles.Count; i++)
            {
                Tile tile = searchSpace.tiles[i];
                if (tile.hasObstacle && tile.obstacle.obstaclePrefab != null)
                {
                    ObstacleInstance obstacle = tile.obstacle;
                    Transform transform = tile.gameObject.transform;

                    InstantiateObstacle(obstacle, transform, levelObstacles.transform);
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
    private bool InstantiateObstacle(ObstacleInstance obstacle, Transform transformParent, Transform parentObject)
    {
       
        Vector3 tempPos = new Vector3(transformParent.transform.position.x, transformParent.transform.position.y + 1, transformParent.transform.position.z);

        if (obstacle.obstaclePrefab.prefab == null)
        {
            return false;
        }
        
        GameObject newObstacle = Instantiate(obstacle.obstaclePrefab.prefab, tempPos, Quaternion.identity, parentObject);
        newObstacle.name = obstacle.obstaclePrefab.name;

        if (obstacle.spawnFrequently)
        {
            obstacle.lastInstance = newObstacle.transform;
        }

        return true;
    }

    /// <summary>
    /// Choose which obstacle to be spawned.
    /// </summary>
    /// <param name="obstacles">List of Obstacles available for lane.</param>
    /// <param name="transformParent">Transform of the tile upon which obstacle will be spawned.</param>
    /// <returns>Obstacle to be spawned. Might return null if</returns>
    private ObstacleInstance ChooseObstacle(List<ObstacleInstance> obstacles, Transform transformParent)
    {
        ObstacleInstance obstacle;
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
                        obstacles[i].lastInstance = transformParent;
                        obstacle = obstacles[i];
                        goto ObstacleChosen;
                    }
                }
                else
                {
                    float dist = Mathf.Abs(obstacles[i].lastInstance.position.z - transformParent.position.z);
                    if (dist >= obstacles[i].spacing)
                    {
                        obstacles[i].lastInstance = transformParent;
                        obstacle = obstacles[i];
                        goto ObstacleChosen;
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
                    obstacle = obstacles[i];
                    goto ObstacleChosen;
                }
            }
            if (preProbability > 1)
            {
                Debug.Log("The sum of all of the randomly spawning obstacle's spawn probability on this lane has to be 100 or less.");
            }
        }


        return null;
    ObstacleChosen:
        return obstacle;
    }


    private bool UsePathfinder(Tile node)
    {
        pathfinder.searchSpace = searchSpace;
        pathfinder.ResetPathfinder();
        return pathfinder.CheckAllPaths(node);
    }

    private bool UsePathfinderOnce(Tile node)
    {
        pathfinder.searchSpace = searchSpace;
        pathfinder.ResetPathfinder();
        return pathfinder.CheckForAPath(node);
    }



    private void OnDrawGizmos()
    {
        if (pathfinder.path.Count > 0)
        {
            Vector3 first = pathfinder.path[0].gameObject.transform.position;
            Vector3 second;
            for (int i = 1; i < pathfinder.path.Count; i++)
            {
                second = pathfinder.path[i].transform.position;

                first += Vector3.up;
                second += Vector3.up;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(first, second);

                first = second - Vector3.up;
            }
        }
    }
}



public class Level : MonoBehaviour
{
    
}

