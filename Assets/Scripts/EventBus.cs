using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public abstract class EventBus<T> where T : Event
    {
        private static event System.Action<T> onEventRaised;

        public static void Subscribe(System.Action<T> action)
        {
            onEventRaised += action;
        }

        public static void Unsubscribe(System.Action<T> action)
        {
            onEventRaised -= action;
        }

        public static void Raise(T eventToRaise)
        {
            onEventRaised?.Invoke(eventToRaise);
        }
    }

    public abstract class Event { }

    public class EnemyRouteEvent : Event
    {
        public Transform[] waypoints;

        public EnemyRouteEvent(Transform[] waypoints)
        {
            this.waypoints = waypoints;
        }
    }

    public class EnemySpawnEvent : Event
    {
        public GameObject enemy;

        public EnemySpawnEvent(GameObject enemy)
        {
            this.enemy = enemy;
        }
    }
}
