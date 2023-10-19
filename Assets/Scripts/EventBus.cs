using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public class RemoveEnemyEvent : Event
    {
        public GameObject enemy;
        public Enemy enemyScript;

        public RemoveEnemyEvent(GameObject enemy)
        {
            this.enemy = enemy;
            this.enemyScript = enemy.GetComponent<Enemy>();
        }
    }

    public class EnemyHitEvent : Event
    {
        public int damage;
        public Enemy enemy;

        public EnemyHitEvent(int damage, Enemy enemy)
        {
            this.damage = damage;
            this.enemy = enemy;
        }
    }

    public class MouseInputEvent : Event
    {
        public PlayerInput.MouseButton mouseButton;
        public MouseInputEvent(PlayerInput.MouseButton mouseButton)
        {
            this.mouseButton = mouseButton;
        }
    }

    public class ChangeGoldEvent : Event
    {
        public int amount;
        
        public ChangeGoldEvent(int amount)
        {
            this.amount = amount;
        }
    }
    
    public class ChangeLivesEvent : Event
    {
        public int amount;
        
        public ChangeLivesEvent(int amount)
        {
            this.amount = amount;
        }
    }

    public class UpdateUIEvent : Event
    {
        public string name;
        
        public UpdateUIEvent(string name)
        {
            this.name = name;
        }
    }
}

