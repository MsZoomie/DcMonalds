using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pathfinder : MonoBehaviour
{
    public SearchSpace searchSpace;
    [Space]

    public List<GameObject> openList = new List<GameObject>();
    public List<GameObject> closedList = new List<GameObject>();
    public List<GameObject> path = new List<GameObject>();
    private List<bool> pathsFound = new List<bool>();

    private bool pathFound = false;
    private bool deadend = false;



    private void Awake()
    {
        searchSpace = FindObjectOfType<SearchSpace>();
    }



    void Update()
    {
        
        pathsFound.Clear();

        for (int i = 0; i < searchSpace.endRow.Count; i++)
        {
            searchSpace.endRow[i].GetComponent<Tile>().isEndNode = true;
            searchSpace.endNode = searchSpace.endRow[i];

            for (int j = 0; j < searchSpace.tiles.Count; j++)
            {
                searchSpace.tiles[j].GetComponent<Tile>().UpdateTile();
            }


            pathsFound.Add(FindPath());

            searchSpace.endRow[i].GetComponent<Tile>().isEndNode = false;
        }

        if (!pathsFound.Contains(true))
        {
            Debug.LogError("There is no available path from player to the end row.");
        }

    }


    public bool FindPath()
    {
        pathFound = false;
        deadend = false;
        ResetLists();


        //Add start node to open list
        for (int i = 0; i < searchSpace.tiles.Count; i++)
        {
            if (searchSpace.tiles[i].GetComponent<Tile>().isStartNode)
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

    void CheckNode(GameObject node)
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

        GameObject leftNode = searchSpace.tiles.Find(x => x.transform.position == leftNodePos);
        GameObject rightNode = searchSpace.tiles.Find(x => x.transform.position == rightNodePos);
        GameObject frontNode = searchSpace.tiles.Find(x => x.transform.position == frontNodePos);


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
    bool AddNodeToOpenList(GameObject node, GameObject parentNode)
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
            node.GetComponent<Tile>().SetParent(parentNode);
            node.GetComponent<Tile>().CalculateCost();
            openList.Add(node);
            return true;
        }
    }

    void CreatePath(GameObject finalNode)
    {
        path.Clear();

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
    }

    /* Obsolete Method ShowOpenList
   public void ShowOpenList()
   {
       foreach (var node in openList)
       {
           if (!path.Contains(node))
           {
               Transform tempTransform = node.transform;
               Vector3 tempPos = new Vector3(tempTransform.position.x, tempTransform.position.y + 0.51f, tempTransform.position.z);
               Quaternion tempQuat = Quaternion.AngleAxis(90.0f, Vector3.right);

               Instantiate(openListIndicator, tempPos, tempQuat, openListParent.transform);
           }
       }
   }
   */

    /*Obsolete Method ShowClosedList
   public void ShowClosedList()
   {
       foreach (var node in closedList)
       {
           if (!path.Contains(node))
           {
               Transform tempTransform = node.transform;
               Vector3 tempPos = new Vector3(tempTransform.position.x, tempTransform.position.y + 0.51f, tempTransform.position.z);
               Quaternion tempQuat = Quaternion.AngleAxis(90.0f, Vector3.right);

               Instantiate(closedListIndicator, tempPos, tempQuat, closedListParent.transform);
           }
       }
   }
   */

    /* Obsolete Method ShowPath
    public void ShowPath()
    {
        for (int i = path.Count - 1; i > -1; i--)
        {
            Transform tempTransform = path[i].transform;
            Vector3 tempPos = new Vector3(tempTransform.position.x, tempTransform.position.y + 0.51f, tempTransform.position.z);
            Quaternion tempQuat = Quaternion.AngleAxis(90.0f, Vector3.right);

            Instantiate(pathIndicator, tempPos, tempQuat, pathParent.transform);

        }
    }
    */

    private void ResetLists()
    {
        openList.Clear();
        closedList.Clear();
       
    }



    public void CheckAllPaths()
    {
        searchSpace = FindObjectOfType<SearchSpace>();

        pathsFound.Clear();

        for (int i = 0; i < searchSpace.endRow.Count; i++)
        {
            searchSpace.endRow[i].GetComponent<Tile>().isEndNode = true;
            searchSpace.endNode = searchSpace.endRow[i];

            for (int j = 0; j < searchSpace.tiles.Count; j++)
            {
                searchSpace.tiles[j].GetComponent<Tile>().UpdateTile();
            }


            pathsFound.Add(FindPath());

            searchSpace.endRow[i].GetComponent<Tile>().isEndNode = false;
        }

        if (!pathsFound.Contains(true))
        {
            Debug.LogError("There is no available path from player to the end row.");
        }

    }
}
