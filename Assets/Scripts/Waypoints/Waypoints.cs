using EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] public Transform[] waypoints;

    private void Start()
    {
        EventBus<EnemyRouteEvent>.Raise(new EnemyRouteEvent(waypoints));
    }
}
