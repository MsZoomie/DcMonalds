using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    public List <GameObject> Tiles = new List<GameObject> ();

    public Transform startNode;
    public Transform endNode;
    
    void Awake()
    {
        BoxCollider[] temp = GetComponentsInChildren<BoxCollider>();
        foreach (var tile in temp)
        {
            Tiles.Add(tile.gameObject);    
            Debug.Log(tile.name);
        }
    }

    void Start()
    {

    }
}