using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
    public override void SetTarget(List<Enemy> enemies)
    {
        this.target = enemies[0].transform;
    }

    public override void Attack()
    {
        //Instantiate a bullet and initialize it with the target and the tower's stats
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().InitializeBullet(bulletSpeed, damage, target.gameObject);
    }
}
