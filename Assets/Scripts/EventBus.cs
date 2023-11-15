using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EventBus {
    public abstract class EventBus<T> where T : Event {
        private static event System.Action<T> onEventRaised;

        public static void Subscribe(System.Action<T> action) {
            onEventRaised += action;
        }

        public static void Unsubscribe(System.Action<T> action) {
            onEventRaised -= action;
        }

        public static void Raise(T eventToRaise) {
            onEventRaised?.Invoke(eventToRaise);
        }
    }

    public abstract class Event { }

    public class EnemyRouteEvent : Event {
        public int path;
        public Transform[] waypoints;

        public EnemyRouteEvent(int path, Transform[] waypoints) {
            this.path = path;
            this.waypoints = waypoints;
        }
    }

    public class EnemySpawnEvent : Event {
        public GameObject enemy;
        public int path;
        public Vector3 spawnPos;
        public int waypointIndex;

        public EnemySpawnEvent(GameObject enemy, int path, Vector3 spawnPos = new Vector3(), int waypointIndex = 0) {
            this.enemy = enemy;
            this.path = path;
            this.spawnPos = spawnPos;
            this.waypointIndex = waypointIndex;
        }
    }

    public class RemoveEnemyEvent : Event {
        public GameObject enemy;
        public Enemy enemyScript;

        public RemoveEnemyEvent(GameObject enemy) {
            this.enemy = enemy;
            this.enemyScript = enemy.GetComponent<Enemy>();
        }
    }

    // public class EnemyHitEvent : Event
    // {
    //     public int damage;
    //     public Enemy enemy;
    //
    //     public EnemyHitEvent(int damage, Enemy enemy)
    //     {
    //         this.damage = damage;
    //         this.enemy = enemy;
    //     }
    // }

    public class MouseInputEvent : Event {
        public PlayerInput.MouseButton mouseButton;
        public Vector2 mousePos;

        public MouseInputEvent(PlayerInput.MouseButton mouseButton, Vector2 mousePos) {
            this.mouseButton = mouseButton;
            this.mousePos = mousePos;
        }
    }

    public class TowerSelectedEvent : Event {
        public Tower tower;
        public bool bought;
        public bool selected;

        public TowerSelectedEvent(Tower tower, bool bought, bool selected) {
            this.tower = tower;
            this.bought = bought;
            this.selected = selected;
        }
    }

    public class TowerPlacedEvent : Event { }

    public class WaveFinishedEvent : Event {
        public WaveFinishedEvent() { }
    }

    public class DisplayPromptEvent : Event {
        public TMPro.TMP_Text promptText;
        public bool display;
        public float time;

        public DisplayPromptEvent(TMP_Text promptText = null, bool display = true, float time = 0f) {
            this.promptText = promptText;
            this.display = display;
            this.time = time;
        }
    }
}