using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Terrain ground;
    GameObject startTransform;
    GameObject endTransform;
    List<GameObject> openList = new List<GameObject>();
    List<GameObject> closedList = new List<GameObject>();

    void Awake()
    {
       startTransform = GameObject.FindWithTag("Player");
       endTransform = GameObject.FindWithTag("Finish");
    }

    // Start is called before the first frame update
    void Start() 
    { 
        //Look for the startNode and add it to the open list
        for (int i = 0; i < ground.Tiles.Count; i++)
        {
            if(ground.Tiles[i].transform.position.x == startTransform. && ground.Tiles[i].transform.position.y == startTransform.y)
            {
               openList.Add(Tiles[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var node in openList)
        {
            
        }
    }
}
