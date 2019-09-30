using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SearchSpace : MonoBehaviour
{
    public List <GameObject> Tiles = new List<GameObject> ();
    public List<GameObject> EndRow = new List<GameObject> ();

    public GameObject endNode;
        
    void Awake()
    {
        Tile[] temp = GetComponentsInChildren<Tile>();
        foreach (var tile in temp)
        {
            Tiles.Add(tile.gameObject);    
        //    Debug.Log(tile.name);
        }
    }
}