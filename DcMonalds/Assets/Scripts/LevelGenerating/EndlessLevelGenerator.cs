using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelGenerator : MonoBehaviour
{
    public GameObject player;
    public float playerOffset;


    [Space]
    public float changeDifficultyTime = 60;

    public Difficulty[] difficulties;

    private Vector3 generationPosition = Vector3.zero;

    private GameObject currentPrefab;

    private void Start()
    {
        generationPosition.z += playerOffset;

        currentPrefab = difficulties[0].levelChunkPrefabs[0];
    }

    private void Update()
    {
       

       if(CheckPlayerPosition())
        {
            GenerateChunk();
        }
    }

    private void GenerateChunk()
    {
        Instantiate(currentPrefab, generationPosition, Quaternion.identity);


        generationPosition.z += playerOffset;
    }

    private bool CheckPlayerPosition()
    {
        float dist = generationPosition.z - player.transform.position.z;

        if (dist <= playerOffset)
        {
            return true;
        }
        return false;
    }

}

[Serializable]
public class Difficulty
{
    public GameObject[] levelChunkPrefabs;
}