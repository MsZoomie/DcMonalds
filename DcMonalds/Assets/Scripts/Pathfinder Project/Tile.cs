using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public bool hasObstacle = false;

    public float tileCost = 0.0f;
    public float heuristicCost = 0.0f;
    public float fromStartCost = 1000.0f;
    
    public Tile parentTile;

    private SearchSpace searchSpace;

    public ObstacleInstance obstacle;

    

    private void Awake()
    {
        searchSpace = FindObjectOfType<SearchSpace>();
    }

    public void UpdateTile()
    {
        ResetTile();
        CalculateHeuristicCost();
    }


    public void AddToStartRow()
    {
        searchSpace.startRow.Add(this);
    }
    public void RemoveFromStartRow()
    {
        searchSpace.startRow.Remove(this);
    }

    public void AddToEndRow()
    {
        searchSpace.endRow.Add(this);
    }
    public void RemoveFromEndRow()
    {
        searchSpace.endRow.Remove(this);
    }

    public bool GetWalkability()
    {
        return !hasObstacle;
    }


    public Tile GetParent()
    {
        return parentTile;
    }
    public void SetParent(Tile node)
    {
        parentTile = node;
    }

    /// <summary>
    /// Should only be used when adding node to the open list.
    /// It is not necessery to calculate the tile cost every time we want to know the cost, 
    /// instead use the GetTileCost method.
    /// </summary>
    /// <returns>tileCost</returns>
    public float CalculateCost()
    {
        if (fromStartCost >= 1000 && (searchSpace.startNode != gameObject))
        {
            fromStartCost = CalculateFromStartCost();
        }
        else if (searchSpace.startNode == gameObject)
        {
            return 0;
        }

        tileCost = fromStartCost + heuristicCost;

        return tileCost;
    }

    /// <summary>
    /// Get the total tile cost without doing calculations.
    /// </summary>
    /// <returns>tileCost</returns>
    public float GetTileCost()
    {
        return tileCost;
    }

    /// <summary>
    /// Checks if there's an obstacle on tile. 
    /// Also checkes if the tile in front of this one has an obstacle that covers more than one tile, 
    /// and in that case sets variable hasObstacle to true and the tilesCovered of the obstacle on this tile.
    /// </summary>
    /// <returns>Wheter there's an obstacle on this tile.</returns>
    public bool CheckForObstacle(int counter)
    {
        counter++;

        if (hasObstacle && counter <= 1)
        {
            return true;
        }


        Vector3 pos = gameObject.transform.position;
        pos += Vector3.back;
        Tile tileInFront = searchSpace.tiles.Find(x => x.gameObject.transform.position == pos);

        //there's no tile in front, therefor there's no obstacle in front
        if (tileInFront == null)
        {
            return false;
        }


        //there's no obstacle in front
        if (!tileInFront.hasObstacle)
        {
            return false;
        }


        //if there is an obstacle in front and it's the first of the tiles it's covering
        if (tileInFront.obstacle.obstaclePrefab != null)
        {
            bool obstacle = false;
            if (tileInFront.obstacle.obstaclePrefab.tilesCovered > 1 && tileInFront.obstacle.obstaclePrefab.tilesCovered > counter)
            {
                obstacle = true;
            }
            return obstacle;
        }
        else    //if there's an obstacle in front and it's not the first of the tiles it's covering
        {
            return tileInFront.CheckForObstacle(counter);
        }
    }


    /// <summary>
    /// Calculate heuristic cost with Manhattan method
    /// </summary>
    private void CalculateHeuristicCost()
    {
        Vector3 endNodePos = searchSpace.endNode.transform.position;
        float x = Mathf.Abs( endNodePos.x - gameObject.transform.position.x);
        float y = Mathf.Abs(endNodePos.z - gameObject.transform.position.z);
        heuristicCost = Mathf.Round( x + y);
    }

    private float CalculateFromStartCost()
    {
        if (searchSpace.startNode == gameObject)
        {
            fromStartCost = 0;
        }
        else if (parentTile == null)
        {
            Debug.Log("You haven't appointed a parent tile for this tile");
        }
        else
        {
            fromStartCost = parentTile.GetComponent<Tile>().CalculateFromStartCost() + 1.0f;
        }

        return fromStartCost;
    }


    private void ResetTile()
    {
        tileCost = 0.0f;
        heuristicCost = 0.0f;
        
        parentTile = null;

        if (gameObject == searchSpace.startNode)
        {
            fromStartCost = 0;
        }
       
    }
}
