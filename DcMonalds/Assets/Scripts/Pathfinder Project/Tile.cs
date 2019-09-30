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

    private SearchSpace searchSpace;

    private void Awake()
    {
        searchSpace = FindObjectOfType<SearchSpace>();
    }

    private void Start()
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, Vector3.forward, out hit, 1))
        {
            searchSpace.EndRow.Add(gameObject);
        }
        else
        {
            Debug.Log("Raycast hit: " + hit, hit.collider.gameObject);
        }

    }

    public void UpdateTile()
    {
        ResetTile();
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


    private void ResetTile()
    {
        isStartNode = false;
        hasObstacle = false;

        tileCost = 0.0f;
        heuristicCost = 0.0f;
        fromStartCost = 1000.0f;

        startNode = null;
        endNode = null;
        parentTile = null;


        startNode = GameObject.FindWithTag("Player");
        if (startNode == null)
            return;
        else if (startNode.transform.position.x == gameObject.transform.position.x &&
            startNode.transform.position.z == gameObject.transform.position.z)
        {
            isStartNode = true;
            fromStartCost = 0;
        }

        endNode = searchSpace.endNode;

        Vector3 up = transform.TransformDirection(Vector3.up);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, up, out hit, 3))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                hasObstacle = true;
            }
        }
    }

}
