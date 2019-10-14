using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level : MonoBehaviour
{
    public string levelName = "Level Name";


    int numberOfLanes;
    


    public int GetNumberOfLanes()
    {
        numberOfLanes = CalculateLanes();
        return numberOfLanes;
    }

    private int CalculateLanes()
    {
        Transform row = transform.GetChild(0);   //rows
        row = row.transform.GetChild(0);        //first row

        int lanes = row.GetComponentsInChildren<Tile>().Length;
        return lanes;
    }
}
