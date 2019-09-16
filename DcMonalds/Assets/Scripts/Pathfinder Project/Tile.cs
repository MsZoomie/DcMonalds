using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    private bool hasObstacle = false;
    public bool isStartNode = false;
    public bool isEndNode = false;

    private GameObject startNode;
    private GameObject endNode;
    private GameObject parentTile;

    void Awake()
    {
        startNode = GameObject.FindWithTag("Player");
        if (startNode == null)
            return;
        else if (startNode.transform.position.x == gameObject.transform.position.x &&
            startNode.transform.position.z == gameObject.transform.position.z)
        {
            isStartNode = true;
        }


        endNode = GameObject.FindWithTag("Finish");
        if (endNode == null)
            return;
        else if (endNode.transform.position.x == gameObject.transform.position.x &&
            endNode.transform.position.z == gameObject.transform.position.z)
        {
            isEndNode = true;
        }
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
}
