using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SearchSpace : MonoBehaviour
{
    public List <GameObject> tiles = new List<GameObject> ();
    public List<GameObject> startRow = new List<GameObject>();
    public List<GameObject> nextRowToCheck = new List<GameObject> ();

    public GameObject endNode;
    public GameObject startNode;
        
   
    public void GetAllTiles()
    {
        tiles.Clear();

        Tile[] temp = GetComponentsInChildren<Tile>();
        foreach (var tile in temp)
        {
            tiles.Add(tile.gameObject);
            //    Debug.Log(tile.name);
        }
    }

    public void AddTile(GameObject node)
    {
        tiles.Add(node);
    }
}