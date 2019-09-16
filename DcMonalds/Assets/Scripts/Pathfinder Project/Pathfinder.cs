﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public SearchSpace searchSpace;
    public GameObject circleSprite;

    List<GameObject> openList = new List<GameObject>();
    List<GameObject> closedList = new List<GameObject>();
    List<GameObject> path = new List<GameObject>();

    bool pathFound = false;


    // Start is called before the first frame update
    void Start() 
    { 
        //Look for the startNode and add it to the open list
        for (int i = 0; i < searchSpace.Tiles.Count; i++)
        {
            if(searchSpace.Tiles[i].GetComponent<Tile>().isStartNode)
            {
               openList.Add(searchSpace.Tiles[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> tempList = new List<GameObject>(openList);

        if (tempList.Count != 0 && !pathFound)
        {
            foreach (var node in tempList)
            {
                if (node.GetComponent<Tile>().isEndNode)
                {
                    pathFound = true;
                    CreatePath(node);
                    return;
                }


                //find adjecent nodes
                Vector3 currentNodePos = node.transform.position;
                Vector3 leftNodePos = new Vector3(currentNodePos.x - 1, currentNodePos.y, currentNodePos.z);
                Vector3 rightNodePos = new Vector3(currentNodePos.x + 1, currentNodePos.y, currentNodePos.z);
                Vector3 frontNodePos = new Vector3(currentNodePos.x, currentNodePos.y, currentNodePos.z + 1);

                GameObject leftNode = searchSpace.Tiles.Find(x => x.transform.position == leftNodePos);
                GameObject rightNode = searchSpace.Tiles.Find(x => x.transform.position == rightNodePos);
                GameObject frontNode = searchSpace.Tiles.Find(x => x.transform.position == frontNodePos);


                //Add the adjecent nodes to the open list if possible
                bool temp = AddNodeToOpenList(leftNode);
                if (temp)
                {
                    leftNode.GetComponent<Tile>().SetParent(node);
                }
                temp = AddNodeToOpenList(rightNode);
                if (temp)
                {
                    rightNode.GetComponent<Tile>().SetParent(node);
                }
                temp = AddNodeToOpenList(frontNode);
                if (temp)
                {
                    frontNode.GetComponent<Tile>().SetParent(node);
                }

                //move current node from the open list to the closed list
                closedList.Add(node);
                openList.Remove(node);
                Debug.Log("Open list count: " + openList.Count);
                Debug.Log("Closed list count: " + closedList.Count);
            }
        }
    }


    /// <summary>
    /// Adds node to open list if possible.
    /// If the gameobject isn't a valid node, return false.
    /// If the closed list contains the node, return false.
    /// If the node isn't walkable, return false.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>Successfully added node to open list.</returns>
    bool AddNodeToOpenList(GameObject node)
    {
        if (node == null)
            return false;

        if (closedList.Contains(node))
            return false;
        else if (openList.Contains(node))
            return false;
        else if (!node.GetComponent<Tile>().GetWalkability())
            return false;
        else
        {
            openList.Add(node);
            return true;
        }
    }



    void CreatePath(GameObject finalNode)
    {
        GameObject currentNode = finalNode;
        while (!currentNode.GetComponent<Tile>().isStartNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetComponent<Tile>().GetParent();
            if (currentNode == null)
            {
                Debug.Log("error creating path");
                return;
            }
        }
        path.Add(currentNode);
        Debug.Log("Path is: ");
        for (int i = path.Count-1; i > -1; i--)
        {
            Transform tempTransform = path[i].transform;
            tempTransform.position = new Vector3(tempTransform.position.x, tempTransform.position.y + 0.51f, tempTransform.position.z);
            tempTransform.Rotate(Vector3.right, 90f);
            tempTransform.localScale = new Vector3(0.01f, 0.01f);
            GameObject indicator = Instantiate(circleSprite, tempTransform);
            Debug.Log(path[i].ToString());
        }
    }
}
