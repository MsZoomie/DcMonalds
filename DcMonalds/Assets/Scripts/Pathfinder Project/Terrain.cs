using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    List <GameObject> Tiles = new List<GameObject> ();

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
