using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SearchSpace : MonoBehaviour
{
    public List <Tile> tiles = new List<Tile> ();
    public List<Tile> startRow = new List<Tile>();
    public List<Tile> endRow = new List<Tile> ();

    public GameObject endNode;
    public GameObject startNode;

    public void GetAllTiles()
    {
        tiles.Clear();

        Tile[] temp = GetComponentsInChildren<Tile>();
        foreach (var tile in temp)
        {
            tiles.Add(tile);
            //    Debug.Log(tile.name);
        }
    }

    public void AddTile(Tile node)
    {
        tiles.Add(node);
    }


    public void UpdateSearchSpace()
    {
        startRow.Clear();
        endRow.Clear();
        GetAllTiles();
    }

    public void UpdateTiles()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].UpdateTile();
        }
    }
}