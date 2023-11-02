using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [FormerlySerializedAs("health")] [SerializeField] private float maxHealth = 5;
    private float health;
    [SerializeField] private int goldDrop = 1;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] Image healthBar;

    private void Start()
    {
        health = maxHealth;
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
    }

    private void CheckPos()
    {
        if(transform.position == waypoints[waypointIndex].position)
        {
            waypointIndex++;
        }
        if(waypointIndex == waypoints.Length)
        {
            Death(true);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Death(false );
        }

        healthBar.fillAmount = (health / maxHealth);
    }

    protected virtual void Death(bool reachedEnd)
    {
        if (!reachedEnd) ResourceManager.changeResourceAction?.Invoke("gold", goldDrop);
        else ResourceManager.changeResourceAction?.Invoke("lives", -1);
        
        EventBus<RemoveEnemyEvent>.Raise(new RemoveEnemyEvent(gameObject));
    }

    public int GetGoldDrop()
    {
        return goldDrop;
    }
}
