using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class MotherEnemy : Enemy
{
    [SerializeField] private GameObject minion;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float spawnDelay = 0.5f;
    

    protected override void Death(bool reachedEnd)
    {
        var m = minion;
        var t = transform.position;
        var p = path;
        var w = waypointIndex;
        if (!reachedEnd)
        {
            EventBus<EnemySpawnEvent>.Raise(new EnemySpawnEvent(m, p, t, w));
            EnemyManager.DelayedForLoop(spawnCount - 1, spawnDelay,
                () =>
                {
                    EventBus<EnemySpawnEvent>.Raise(new EnemySpawnEvent(m, p, t, w));
                }, 
                EnemyManager.GetInstanceFunc());
        }

        base.Death(reachedEnd);
    }
}