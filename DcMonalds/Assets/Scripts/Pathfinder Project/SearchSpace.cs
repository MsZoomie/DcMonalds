using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSpace : MonoBehaviour
{
    public List <GameObject> Tiles = new List<GameObject> ();
        
    void Awake()
    {
        Tile[] temp = GetComponentsInChildren<Tile>();
        foreach (var tile in temp)
        {
            Tiles.Add(tile.gameObject);    
        //    Debug.Log(tile.name);
        }
    }

    void Start()
    {

    }
}