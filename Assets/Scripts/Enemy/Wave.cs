using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] public List<Subwave> subwaves;
    public int waveReward;
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
