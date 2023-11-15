using System;
using EventBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = EventBus.Event;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private List<Transform[]> paths;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private List<Enemy> enemies;
    private int pos = 0;

    public static Func<List<Enemy>> getEnemiesFunc;

    private void OnEnable() {
        getEnemiesFunc += getEnemies;

        EventBus<EnemyRouteEvent>.Subscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Subscribe(SpawnEnemy);
        EventBus<RemoveEnemyEvent>.Subscribe(RemoveEnemy);
        EventBus<EnemyHitEvent>.Subscribe(DamageEnemy);
    }

    private void OnDisable() {
        getEnemiesFunc -= getEnemies;

        EventBus<EnemyRouteEvent>.Unsubscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Unsubscribe(SpawnEnemy);
        EventBus<RemoveEnemyEvent>.Unsubscribe(RemoveEnemy);
        EventBus<EnemyHitEvent>.Unsubscribe(DamageEnemy);
    }

    private List<Enemy> getEnemies() {
        return enemies;
    }

    private void Awake() {
        paths = new List<Transform[]>();
    }

    private void SetEnemyRoute(EnemyRouteEvent e) {
        //If we already know a path, insert the new path into the list at the correct position.
        if (paths.Count > 0) paths.Insert(e.path, e.waypoints);
        //If this is the first path, add it.
        else paths.Add(e.waypoints);
    }

    public void SpawnEnemy(EnemySpawnEvent e) {
        //Instantiate the enemy at the first waypoint of the path that the event tells us.
        GameObject enemy = Instantiate(e.enemy, paths[e.path][0].position, Quaternion.identity);

        //Retrieve the enemy script from the enemy gameobject and set the waypoints.
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetWaypoints(paths[e.path]);

        //Add the enemy to the list of enemies.
        enemies.Add(enemyScript);
    }

    public void RemoveEnemy(RemoveEnemyEvent e) {
        enemies.Remove(e.enemyScript);
        Destroy(e.enemy);

        if (enemies.Count == 0 && WaveManager.getWaveInProgressFunc?.Invoke() == false) {
            EventBus<WaveFinishedEvent>.Raise(new WaveFinishedEvent());
        }
    }

    public void DamageEnemy(EnemyHitEvent e) {
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i] == e.enemy) enemies[i].TakeDamage(e.damage);
        }
    }
}