using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private int health = 5;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] float routeProgress = 0f;

    private void Start()
    {
        transform.position = new Vector3(waypoints[waypointIndex].position.x, waypoints[waypointIndex].position.y, 0);
    }

    private void Update()
    {
        Move();
        CheckPos();
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);
        routeProgress++;
    }

    private void CheckPos()
    {
        if(transform.position == waypoints[waypointIndex].position)
        {
            waypointIndex++;
        }
        if(waypointIndex == waypoints.Length)
        {
            Death();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        EventBus<RemoveEnemyEvent>.Raise(new RemoveEnemyEvent(gameObject));
    }

    public float GetRouteProgress()
    {
        return routeProgress;
    }
}
