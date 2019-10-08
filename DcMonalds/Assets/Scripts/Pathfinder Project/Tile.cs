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
    
    public GameObject parentTile;

    private SearchSpace searchSpace;

    public Obstacle obstacle;

    private void Awake()
    {
        searchSpace = FindObjectOfType<SearchSpace>();
    }

    private void Start()
    {
        /*
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, 3))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                hasObstacle = true;
            }
        }
        */
    }

    public void UpdateTile()
    {
        ResetTile();
        CalculateHeuristicCost();
        CalculateCost();
    }


    public void AddToStartRow()
    {
        searchSpace.startRow.Add(gameObject);
    }
    public void RemoveFromStartRow()
    {
        searchSpace.startRow.Remove(gameObject);
    }

    public void AddToEndRow()
    {
        searchSpace.endRow.Add(gameObject);
    }
    public void RemoveFromEndRow()
    {
        searchSpace.endRow.Remove(gameObject);
    }

    public bool GetWalkability()
    {
        return !hasObstacle;
    }
    public void SetWalkability(bool isWalkable)
    {
        hasObstacle = !isWalkable;
    }


    public GameObject GetParent()
    {
        return parentTile;
    }
    public void SetParent(GameObject node)
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
        //hasObstacle = false;

        tileCost = 0.0f;
        heuristicCost = 0.0f;
        fromStartCost = 1000.0f;
        
        parentTile = null;

        if (gameObject == searchSpace.startNode)
        {
            fromStartCost = 0;
        }
        
       /* RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 3))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                hasObstacle = true;
            }
        }*/
    }

}
