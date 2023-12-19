using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] public List<Path> paths;
    public int waveReward;

    public void Reset()
    {
        foreach(Path path in paths)
        {
            path.currentSubwaveIndex = 0;
        }
    }
}

[System.Serializable]
public class Path
{
    public int currentSubwaveIndex = 0;
    [SerializeField] public List<Subwave> subwaves;
}

[System.Serializable]
public class Subwave
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int amountToSpawn;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float timeBetweenSubwaves;

    public GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }

    public int GetAmountToSpawn()
    {
        return amountToSpawn;
    }

    public float GetTimeBetweenSpawns()
    {
        return timeBetweenSpawns;
    }

    public float GetTimeBetweenSubwaves()
    {
        return timeBetweenSubwaves;
    }
}
