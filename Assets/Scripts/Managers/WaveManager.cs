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
    private bool canStartWave = true;
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
        
        //Unity sucks and doesn't reset the current subwave index when exiting play mode, so we have to do it ourselves
        foreach(Wave w in waves)
        {
            w.Reset();
        }
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
        if (!waveInProgress && canStartWave)
        {
            //Reset the tower attacked time
            EventBus<ResetTowerAttackedTimeEvent>.Raise(new ResetTowerAttackedTimeEvent());
            
            //We must start a coroutine for each separate path.
            int p = 0;
            foreach (Path path in waves[currentWaveIndex].paths)
            {
                //If we have reached the last path, start the wave as normal
                if (p == waves[currentWaveIndex].paths.Count - 1)
                {
                    StartCoroutine(SpawnWave(p));
                }
                //Else, start the wave but don't increment the wave index because another path will be spawned
                else
                {
                    StartCoroutine(SpawnWave(p, false));
                    p++;
                }
            }
        }
    }

    private IEnumerator SpawnWave(int p, bool incrementWave = true)
    {
        //Set the wave in progress to true, so that the player can't start another wave while one is in progress
        waveInProgress = true;
        canStartWave = false;
        

        
        //For each subwave in the current wave...
        for (int i = 0; i < waves[currentWaveIndex].paths[p].subwaves.Count; i++)
        {
            int wave = currentWaveIndex;
            int subwave = waves[currentWaveIndex].paths[p].currentSubwaveIndex;
            
            //spawn an enemy...
            for (int spawn = 0; spawn < waves[wave].paths[p].subwaves[subwave].GetAmountToSpawn(); spawn++)
            {
                GameObject enemy = waves[wave].paths[p].subwaves[subwave].GetEnemyPrefab();
                EventBus<EnemySpawnEvent>.Raise(new EnemySpawnEvent(enemy, p));

                //and wait for the time between spawns before spawning the next enemy
                yield return new WaitForSeconds(waves[wave].paths[p].subwaves[subwave].GetTimeBetweenSpawns());
            }
            
            //If there is another subwave in the current wave
            if (subwave + 1 < waves[wave].paths[p].subwaves.Count)
            {
                //Wait for the time between subwaves
                yield return new WaitForSeconds(waves[wave].paths[p].subwaves[subwave].GetTimeBetweenSubwaves());

                //Check again if there is another subwave, because the player may have started another wave while we were waiting
                //Then increment the subwave index.
                if (currentSubwaveIndex + 1 < waves[wave].paths[p].subwaves.Count)
                {
                    waves[currentWaveIndex].paths[p].currentSubwaveIndex++;
                }
            }
            //If there is no next subwave but there is another wave, increment the wave index and reset the subwave index
            else if (wave + 1 < waves.Count && incrementWave)
            {
                waveInProgress = false;
            }
            //Else, the level is complete and we don't need to do anything
            else
            {
                waveInProgress = false;
                Debug.Log("Level complete");
            }
            

        }
    }

    public bool GetWaveInProgress()
    {
        return waveInProgress;
    }

    public void OnWaveFinished(WaveFinishedEvent e)
    {
        //If the player is already able to start another wave, return
        if(canStartWave) return;
        
        //If there is another wave, increment the wave index and reset the subwave index
        if(ResourceManager.getResourceValueFunc?.Invoke("totalWaves") >= ResourceManager.getResourceValueFunc?.Invoke("waves") + 1)
        {
            currentWaveIndex++;
            
            foreach(Path p in waves[currentWaveIndex - 1].paths)
            {
                p.currentSubwaveIndex = 0;
            }

            //The player gets a reward for completing the wave
            ResourceManager.changeResourceAction?.Invoke("gold", waves[currentWaveIndex - 1].waveReward);
            ResourceManager.changeResourceAction?.Invoke("waves", 1);
        }
        //Else, the level is complete
        else
        {
            EventBus<LevelCompleteEvent>.Raise(new LevelCompleteEvent(true));
        }
        
        //Let the player start another wave
        canStartWave = true;
    }
}
