using System.Collections;
using System.Collections.Generic;
using EventBus;
using Unity.Android.Types;
using UnityEngine;

public class SniperTower : Tower {
    public override void SetTarget(List<Enemy> enemies) {
        target = enemies[0].transform;
    }


    public override void Attack() {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeBullet(bulletSpeed, damage, target.gameObject);
    }
}