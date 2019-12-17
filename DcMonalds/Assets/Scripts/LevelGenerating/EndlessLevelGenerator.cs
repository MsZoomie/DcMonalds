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
    private float lastDifficultyChangeTime;
    private int currentDifficulty;

    private void Start()
    {
        currentDifficulty = 0;
        lastDifficultyChangeTime = Time.time;

        generationPosition.z += playerOffset;
    }

    private void Update()
    {
        if(CheckPlayerPosition())
        {
            GenerateChunk();
        }

        if(Time.time - lastDifficultyChangeTime >= changeDifficultyTime)
        {
            ChangeDifficulty();
        }
    }

    private void GenerateChunk()
    {
        GameObject chunkToGenerate = ChooseLevelChunk();

        Instantiate(chunkToGenerate, generationPosition, Quaternion.identity);

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

    private void ChangeDifficulty()
    {
        if (difficulties.Length - 1 <= currentDifficulty)
            return;

        currentDifficulty++;
        lastDifficultyChangeTime = Time.time;
        Debug.Log("changing difficulty");
    }

    private GameObject ChooseLevelChunk()
    {
        GameObject levelChunk;

        int rand = UnityEngine.Random.Range(0, difficulties[currentDifficulty].levelChunkPrefabs.Length);

        levelChunk = difficulties[currentDifficulty].levelChunkPrefabs[rand];

        return levelChunk;
    }

}



[Serializable]
public class Difficulty
{
    public GameObject[] levelChunkPrefabs;
}