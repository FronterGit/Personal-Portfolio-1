using EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private int pathIndex = 0;

    private void Start()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            waypoints.Add(t);
        }
        waypoints.Remove(transform);
        EventBus<EnemyRouteEvent>.Raise(new EnemyRouteEvent(pathIndex, waypoints.ToArray()));
    }
}
