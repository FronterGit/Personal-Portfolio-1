using System;
using EventBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = EventBus.Event;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Transform[]> paths;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private List<Enemy> enemies;
    private int pos = 0;

    public static Func<EnemyManager> GetInstanceFunc;

    private void OnEnable()
    {
        EventBus<EnemyRouteEvent>.Subscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Subscribe(SpawnEnemy);
        EventBus<RemoveEnemyEvent>.Subscribe(RemoveEnemy);
        //EventBus<EnemyHitEvent>.Subscribe(DamageEnemy);
        
        GetInstanceFunc += () => this;
    }

    private void OnDisable()
    {
        EventBus<EnemyRouteEvent>.Unsubscribe(SetEnemyRoute);
        EventBus<EnemySpawnEvent>.Unsubscribe(SpawnEnemy);
        EventBus<RemoveEnemyEvent>.Unsubscribe(RemoveEnemy);
        //EventBus<EnemyHitEvent>.Unsubscribe(DamageEnemy);
        
        GetInstanceFunc -= () => this;
    }

    private void Awake()
    {
        paths = new List<Transform[]>();
    }

    private void SetEnemyRoute(EnemyRouteEvent e)
    {
        //If we already know a path, insert the new path into the list at the correct position.
        if (paths.Count > 0) paths.Insert(e.path, e.waypoints);
        //If this is the first path, add it.
        else paths.Add(e.waypoints);
    }

    public void SpawnEnemy(EnemySpawnEvent e)
    {
        GameObject enemy;
        Enemy enemyScript;

        if (e.spawnPos == Vector3.zero)
        {
            //Instantiate the enemy at the first waypoint of the path that the event tells us.
            enemy = Instantiate(e.enemy, paths[e.path][0].position, Quaternion.identity);
        }
        else
        {
            //Instantiate the enemy at the given spawn position that the event tells us.
            enemy = Instantiate(e.enemy, e.spawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetWaypointIndex(e.waypointIndex);
        }

        //Retrieve the enemy script from the enemy gameobject and set the waypoints.
        enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetWaypoints(paths[e.path]);
        enemyScript.SetPath(e.path);

        //Add the enemy to the list of enemies.
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

    // public void DamageEnemy(EnemyHitEvent e)
    // {
    //     for (int i = 0; i < enemies.Count; i++)
    //     {
    //         if(enemies[i] == e.enemy) enemies[i].TakeDamage(e.damage);
    //     }
    // }

    public static void DelayedForLoop(int iterations, float delay, Action action, MonoBehaviour context)
    {
        for (int i = 0; i < iterations; i++)
        {
            InvokeDelayed(() => { action.Invoke(); }, delay * i, context);
        }
    }

    public static void InvokeDelayed(System.Action action, float delay, MonoBehaviour context)
    {
        context.StartCoroutine(InvokeDelayedCoroutine(action, delay));
    }

    private static IEnumerator InvokeDelayedCoroutine(System.Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}