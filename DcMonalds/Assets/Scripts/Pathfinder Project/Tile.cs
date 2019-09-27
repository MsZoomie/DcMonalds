using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public bool isStartNode = false;
    public bool isEndNode = false;
    public bool hasObstacle = false;

    public float tileCost = 0.0f;
    public float heuristicCost = 0.0f;
    public float fromStartCost = 1000.0f;

    private GameObject startNode;
    private GameObject endNode;
    public GameObject parentTile;


    void Awake()
    {
        startNode = GameObject.FindWithTag("Player");
        if (startNode == null)
            return;
        else if (startNode.transform.position.x == gameObject.transform.position.x &&
            startNode.transform.position.z == gameObject.transform.position.z)
        {
            isStartNode = true;
            fromStartCost = 0;
        }


        endNode = GameObject.FindWithTag("Finish");
        if (endNode == null)
            return;
        else if (endNode.transform.position.x == gameObject.transform.position.x &&
            endNode.transform.position.z == gameObject.transform.position.z)
        {
            isEndNode = true;
        }


        Vector3 up = transform.TransformDirection(Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, up, out hit, 3))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                hasObstacle = true;
            }
        }
        

        CalculateHeuristicCost();
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
        if (fromStartCost >= 1000 && !isStartNode)
        {
            fromStartCost = CalculateFromStartCost();
        }
        else if (isStartNode)
        {
            Debug.Log("This is the start node, it doesn't have a parent node.");
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

    private void CalculateHeuristicCost()
    {
        float x = Mathf.Abs( endNode.transform.position.x - gameObject.transform.position.x);
        float y = Mathf.Abs(endNode.transform.position.z - gameObject.transform.position.z);
        heuristicCost = Mathf.Round( x + y);
    }

    private float CalculateFromStartCost()
    {
        if (isStartNode)
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
}
