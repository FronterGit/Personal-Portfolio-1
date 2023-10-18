using System;
using EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] public float range = 2f;
    [SerializeField] public int damage = 1;
    [SerializeField] public float fireRate = 1f;
    [SerializeField] public float bulletSpeed = 5f;
    public int cost = 1;
    [SerializeField] public string towerName = "Tower";

    [Header("Unity Setup Fields")]
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject placePrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform target;
    [SerializeField] public CircleCollider2D rangeCollider;
  
    [SerializeField] public List<Enemy> enemiesInRange;

    private void Start()
    {
        //Set the range of the tower
        rangeCollider.radius = range;

        //Convert fireRate to shots per second
        fireRate = 1 / fireRate;
    }

    private IEnumerator FireCooldown(float wait)
    {
        //If there are enemies in range...
        if (enemiesInRange.Count > 0)
        {
            //...set the target to the first enemy in range,
            SetTarget(enemiesInRange);

            if (target != null)
            {
                Attack();
            }

            //Wait for the fireRate before shooting again
            yield return new WaitForSeconds(wait);
            StartCoroutine(FireCooldown(fireRate));
        }
        else yield return null;
    }

    #region Tower Methods
    public abstract void SetTarget(List<Enemy> enemies);

    public abstract void Attack();
    #endregion

    #region Enemies in Range
    //Keep track of enemies in range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If an enemy enters the tower's range, add it to the list of enemies in range
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.GetComponent<Enemy>());

            //If this is the first enemy to come into range, start shooting
            if(enemiesInRange.Count == 1) StartCoroutine(FireCooldown(fireRate));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If an enemy leaves the tower's range, remove it from the list of enemies in range
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<Enemy>());

            //If there are no more enemies in range, stop shooting
            if (enemiesInRange.Count == 0) StopAllCoroutines();
        }
    }
    #endregion
    
    #region Getters and Setters
    public GameObject GetPlacePrefab()
    {
        return placePrefab;
    }
    #endregion

    public void SetCost(int amount)
    {
        cost = amount;
    }
}
