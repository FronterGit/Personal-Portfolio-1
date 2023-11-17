using System;
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
    public bool waveInProgress = false;
    public static Func<bool> getWaveInProgressFunc;

    private void OnEnable()
    {
        getWaveInProgressFunc += GetWaveInProgress;
        EventBus<WaveFinishedEvent>.Subscribe(OnWaveFinished);
    }

    private void OnDisable()
    {
        getWaveInProgressFunc -= GetWaveInProgress;
        EventBus<WaveFinishedEvent>.Unsubscribe(OnWaveFinished);
    }

    private void Start()
    {
        ResourceManager.changeResourceAction?.Invoke("totalWaves", waves.Count);
    }

    //Method called by the start wave button in the UI
    public void StartWave()
    {
        Debug.Log("Starting wave");
        //If there is no wave in progress, start the wave
        if (!waveInProgress)
        {
            //We must start a coroutine for each separate path.
            int p = 0;
            foreach (Path path in waves[currentWaveIndex].paths)
            {
                StartCoroutine(SpawnWave(p));
                p++;
            }
        }
    }

    private IEnumerator SpawnWave(int p)
    {
        //Set the wave in progress to true, so that the player can't start another wave while one is in progress
        waveInProgress = true;
        
        //For each subwave in the current wave...
        for (int i = 0; i < waves[currentWaveIndex].paths[p].subwaves.Count; i++)
        {
            //spawn an enemy...
            for (int spawn = 0; spawn < waves[currentWaveIndex].paths[p].subwaves[currentSubwaveIndex].GetAmountToSpawn(); spawn++)
            {
                GameObject enemy = waves[currentWaveIndex].paths[p].subwaves[currentSubwaveIndex].GetEnemyPrefab();
                EventBus<EnemySpawnEvent>.Raise(new EnemySpawnEvent(enemy, p));

                //and wait for the time between spawns before spawning the next enemy
                yield return new WaitForSeconds(waves[currentWaveIndex].paths[p].subwaves[currentSubwaveIndex].GetTimeBetweenSpawns());
            }
            
            //If there is another subwave in the current wave
            if (currentSubwaveIndex + 1 < waves[currentWaveIndex].paths[p].subwaves.Count)
            {
                //Wait for the time between subwaves
                yield return new WaitForSeconds(waves[currentWaveIndex].paths[p].subwaves[currentSubwaveIndex].GetTimeBetweenSubwaves());

                //Then increment the subwave index.
                currentSubwaveIndex++;
            }

            //If there is no next subwave but there is another wave, increment the wave index and reset the subwave index
            else if (currentWaveIndex + 1 < waves.Count)
            {
                currentWaveIndex++;
                currentSubwaveIndex = 0;
                        
                //Set the wave in progress to false, so that the player can start another wave when all enemies are dead.
                waveInProgress = false;
                
                break;
            }
            else
            { 
                Debug.Log("Level complete");
            }
        }
    }

    public bool GetWaveInProgress()
    {
        Debug.Log("Wave in progress: " + waveInProgress);
        return waveInProgress;
    }

    public void OnWaveFinished(WaveFinishedEvent e)
    {
        ResourceManager.changeResourceAction?.Invoke("gold", waves[currentWaveIndex - 1].waveReward);
        ResourceManager.changeResourceAction?.Invoke("waves", 1);
    }
}
