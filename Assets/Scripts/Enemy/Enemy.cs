using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected float maxHealth = 5;
    protected float health;
    [SerializeField] private int goldDrop = 1;
    [SerializeField] protected int damage = 1;
    protected int path;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] protected int waypointIndex = 0;
    [SerializeField] Image healthBar;

    protected virtual void Start()
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
    
    public void SetPath(int path)
    {
        this.path = path;
    }
    
    public void SetWaypointIndex(int waypointIndex)
    {
        this.waypointIndex = waypointIndex;
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

    public virtual void TakeDamage(int damage)
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
        else ResourceManager.changeResourceAction?.Invoke("lives", damage);
        
        EventBus<RemoveEnemyEvent>.Raise(new RemoveEnemyEvent(gameObject));
    }

    public int GetGoldDrop()
    {
        return goldDrop;
    }
}
