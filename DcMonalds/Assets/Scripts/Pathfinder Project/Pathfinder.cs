using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pathfinder : MonoBehaviour
{
    public SearchSpace searchSpace;
    [Space]

    public List<Tile> openList = new List<Tile>();
    public List<Tile> closedList = new List<Tile>();
    public List<Tile> path = new List<Tile>();
    private List<bool> pathsFound = new List<bool>();

    private bool pathFound = false;
    private bool deadend = false;
    

    /// <summary>
    /// Finds a path from start node to end node.
    /// </summary>
    /// <returns>true if there's a path. false if no path could be found.</returns>
    public bool FindPath()
    {
        pathFound = false;
        deadend = false;
        ResetLists();


        //Add start node to open list
        for (int i = 0; i < searchSpace.tiles.Count; i++)
        {
            if (searchSpace.tiles[i] == searchSpace.startNode)
            {
                openList.Add(searchSpace.tiles[i]);
            }
        }

        
        //find a path
        while (!pathFound && !deadend)
        {

            List<GameObject> tempList = new List<GameObject>(openList);

            GameObject nodeWithSmallestCost = null;
            float currentSmallestCost = 10000;   //using 1000 since the search space isn't very large, if using a larger search spece, increase currentSmallestCost

            //dead end
            if (tempList.Count <= 0)
            {
                deadend = true;
                //Debug.Log("deadend");
                return !deadend;
            }
            else    //not dead end
            {
                foreach (var node in tempList)
                {
                    float tempCost = node.GetComponent<Tile>().CalculateCost();
                    currentSmallestCost = Mathf.Min(currentSmallestCost, tempCost);
                    if (currentSmallestCost == tempCost)
                    {
                        nodeWithSmallestCost = node;
                    }
                }

                if (nodeWithSmallestCost != null)
                    CheckNode(nodeWithSmallestCost);
            }
        }
        return !deadend;
    }

    private void CheckNode(GameObject node)
    {
        if (searchSpace.endNode == node)
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

        Tile leftNode = searchSpace.tiles.Find(x => x.gameObject.transform.position == leftNodePos);
        Tile rightNode = searchSpace.tiles.Find(x => x.transform.position == rightNodePos);
        Tile frontNode = searchSpace.tiles.Find(x => x.transform.position == frontNodePos);

        
        //Add the adjecent nodes to the open list if possible
        AddNodeToOpenList(leftNode, node);
        AddNodeToOpenList(rightNode, node);
        AddNodeToOpenList(frontNode, node);


        //move current node from the open list to the closed list
        closedList.Add(node);
        openList.Remove(node);
    }

    /// <summary>
    /// Adds node to open list if possible.
    /// If the gameobject isn't a valid node, return false.
    /// If the closed list contains the node, return false.
    /// If the node isn't walkable, return false.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>Successfully added node to open list.</returns>
    private bool AddNodeToOpenList(Tile node, Tile parentNode)
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
            node.SetParent(parentNode);
            node.CalculateCost();
            openList.Add(node);
            return true;
        }
    }

    private void CreatePath(GameObject finalNode)
    {
        path.Clear();

        GameObject currentNode = finalNode;
        while (currentNode != searchSpace.startNode )
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
    }
    
    private void ResetLists()
    {
        openList.Clear();
        closedList.Clear();
    }


    /// <summary>
    /// Checks if theres any paths from start row to current node
    /// </summary>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    public bool CheckAllPaths(GameObject currentNode)
    {
        pathsFound.Clear();
        searchSpace.endNode = currentNode;

        for (int i = 0; i < searchSpace.startRow.Count; i++)
        {
            searchSpace.startNode = searchSpace.startRow[i].gameObject;
            bool found = FindPath();
            pathsFound.Add(found);
        }

        if (!pathsFound.Contains(true))
        {
            //Debug.LogError("There is no available path from start row to current tile.", currentNode);
            return false;
        }

        return true;
    }


    public bool CheckForAPath(GameObject currentNode)
    {
        pathsFound.Clear();
        searchSpace.endNode = currentNode;

        for (int i = 0; i < searchSpace.startRow.Count; i++)
        {
            searchSpace.startNode = searchSpace.startRow[i].gameObject;
            bool found = FindPath();
            if (found)
            {
                return true;
            }
        }

        return false;
    }


    public void ResetPathfinder()
    {
        openList.Clear();
        closedList.Clear();
        path.Clear();
        pathsFound.Clear();

        pathFound = false;
        deadend = false;
    }


}
