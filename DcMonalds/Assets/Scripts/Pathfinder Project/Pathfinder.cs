using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public SearchSpace searchSpace;
    [Space]
    public GameObject openListIndicator;
    public GameObject openListParent;
    [Space]
    public GameObject closedListIndicator;
    public GameObject closedListParent;
    [Space]
    public GameObject pathIndicator;
    public GameObject pathParent;

    List<GameObject> openList = new List<GameObject>();
    List<GameObject> closedList = new List<GameObject>();
    List<GameObject> path = new List<GameObject>();

    private PlayerMovement playerMovement;

    private bool pathFound = false;
    //private bool listsIndicated = false;


    private void Awake()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }


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



    public void FindPath()
    {
        while (!pathFound)
        {
            List<GameObject> tempList = new List<GameObject>(openList);

            GameObject nodeWithSmallestCost = null;
            float currentSmallestCost = 1000;   //using 1000 since the search space isn't very large, if using a larger search spece, increase currentSmallestCost

            if (tempList.Count != 0 && !pathFound)
            {
                foreach (var node in tempList)
                {
                    float tempCost = node.GetComponent<Tile>().GetTileCost();
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
        //   Debug.Log("Open list count: " + openList.Count);
        //   Debug.Log("Closed list count: " + closedList.Count);
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
            node.GetComponent<Tile>().CalculateCost();
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
    }

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

    public void MoveAgent()
    {
        if (!pathFound)
        {
            Debug.Log("Agent cannot move before a path is created.");
            return;
        }

        List<PlayerMovement.Direction> pathDirections = new List<PlayerMovement.Direction>();

        for (int i = path.Count - 2; i > 0; i--)
        {
            Vector3 dir = path[i + 1].transform.position - path[i].transform.position;

            if (Mathf.Approximately (dir.x, -1))
            {
                pathDirections.Add(PlayerMovement.Direction.LEFT);
            }
            else if (Mathf.Approximately (dir.x, 1))
            {
                pathDirections.Add(PlayerMovement.Direction.RIGHT);
            }
            else if(Mathf.Approximately (dir.z, 1))
            {
                pathDirections.Add(PlayerMovement.Direction.FORWARD);
            }

            Debug.Log(pathDirections[path.Count - i + 1].ToString());
        }

    //    playerMovement.CrossyRoadMove();
    }
}
