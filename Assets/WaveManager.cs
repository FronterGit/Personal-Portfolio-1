using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int currentSubwaveIndex = 0;

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < waves[currentWaveIndex].subwaves.Count; i++)
        {
            for (int spawn = 0; spawn < waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetAmountToSpawn(); spawn++)
            {
                GameObject enemy = waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetEnemyPrefab();
                EventBus<EnemySpawnEvent>.Raise(new EnemySpawnEvent(enemy));
                yield return new WaitForSeconds(waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetTimeBetweenSpawns());
            }
            yield return new WaitForSeconds(waves[currentWaveIndex].subwaves[currentSubwaveIndex].GetTimeBetweenSubwaves());

            if (waves[currentWaveIndex].subwaves[currentSubwaveIndex + 1] != null) currentSubwaveIndex++;
            else if (waves[currentWaveIndex + 1] != null)
            {
                currentWaveIndex++;
                currentSubwaveIndex = 0;
            }
        }
    }
}
