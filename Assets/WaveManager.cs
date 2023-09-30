using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

//This script is responsible for telling the enemy manager to when to spawn enemies and what enemies to spawn
public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int currentSubwaveIndex = 0;

    //Method called by the start wave button in the UI
    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        //For each subwave in the current wave...
        for (int i = 0; i < waves[currentWaveIndex].subwaves.Count; i++)
        {
            //...spawn an enemy...
            for (int spawn = 0; spawn < waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetAmountToSpawn(); spawn++)
            {
                GameObject enemy = waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetEnemyPrefab();
                EventBus<EnemySpawnEvent>.Raise(new EnemySpawnEvent(enemy));

                //...and wait for the time between spawns before spawning the next enemy
                yield return new WaitForSeconds(waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetTimeBetweenSpawns());
            }

            //Then wait for the time between subwaves before spawning the next subwave
            yield return new WaitForSeconds(waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetTimeBetweenSubwaves());


            //If there is another subwave in the current wave, increment the subwave index
            if (currentSubwaveIndex + 1 < waves[currentWaveIndex].subwaves.Count) currentSubwaveIndex++;

            //If there is no next subwave but there is another wave, increment the wave index and reset the subwave index
            else if (currentWaveIndex + 1 < waves.Count)
            {
                currentWaveIndex++;
                currentSubwaveIndex = 0;
            }
            else
            {
                Debug.Log("Level complete");
            }
        }
    }
}
