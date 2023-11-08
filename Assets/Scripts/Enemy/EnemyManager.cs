using EventBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = EventBus.Event;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private List<Enemy> enemies;

    private void OnEnable()
    {
        EventBus<EnemyRouteEvent>.Subscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Subscribe(SpawnEnemy);
        EventBus<RemoveEnemyEvent>.Subscribe(RemoveEnemy);
        EventBus<EnemyHitEvent>.Subscribe(DamageEnemy);
    }

    private void OnDisable()
    {
        EventBus<EnemyRouteEvent>.Unsubscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Unsubscribe(SpawnEnemy);
        EventBus<RemoveEnemyEvent>.Unsubscribe(RemoveEnemy);
        EventBus<EnemyHitEvent>.Unsubscribe(DamageEnemy);
    }

    private void SetEnemyRoute(EnemyRouteEvent e)
    {
        this.waypoints = e.waypoints;
    }

    public void SpawnEnemy(EnemySpawnEvent e)
    {
        GameObject enemy = Instantiate(e.enemy, waypoints[0].position, Quaternion.identity);
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetWaypoints(waypoints);
        
        enemies.Add(enemyScript);
    }

    public void RemoveEnemy(RemoveEnemyEvent e)
    {
        enemies.Remove(e.enemyScript);
        Destroy(e.enemy);

        if (enemies.Count == 0 && WaveManager.getWaveInProgressFunc?.Invoke() == false)
        {
            EventBus<WaveFinishedEvent>.Raise(new WaveFinishedEvent());
        }
    }

    public void DamageEnemy(EnemyHitEvent e)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] == e.enemy) enemies[i].TakeDamage(e.damage);
        }
    }
}
