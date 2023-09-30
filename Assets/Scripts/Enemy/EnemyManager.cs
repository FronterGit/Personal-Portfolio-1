using EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private List<Enemy> enemies;

    private void OnEnable()
    {
        EventBus<EnemyRouteEvent>.Subscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Subscribe(SpawnEnemy);
    }

    private void OnDisable()
    {
        EventBus<EnemyRouteEvent>.Unsubscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Unsubscribe(SpawnEnemy);
    }

    private void SetEnemyRoute(EnemyRouteEvent e)
    {
        this.waypoints = e.waypoints;
    }

    public void SpawnEnemy(EnemySpawnEvent e)
    {
        GameObject enemy = Instantiate(e.enemy, waypoints[0].position, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetWaypoints(waypoints);
        enemies.Add(enemy.GetComponent<Enemy>());
    }
}
