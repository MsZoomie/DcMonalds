using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SearchSpace : MonoBehaviour
{
    public List <GameObject> tiles = new List<GameObject> ();
    public List<GameObject> startRow = new List<GameObject>();
    public List<GameObject> endRow = new List<GameObject> ();

    public GameObject endNode;
        
    void Awake()
    {
        Tile[] temp = GetComponentsInChildren<Tile>();
        foreach (var tile in temp)
        {
            tiles.Add(tile.gameObject);    
        //    Debug.Log(tile.name);
        }
    }
}