using System.Collections;
using System.Collections.Generic;
using EventBus;
using Unity.Android.Types;
using UnityEngine;

public class SniperTower : Tower {
    public override void SetTarget(List<Enemy> enemies) {
        target = enemies[0].transform;
    }

    protected override void OnEnable() {
        base.OnEnable();

        EnemyManager.GetInstanceFunc().onFirstEnemySpawned += onFirstEnemySpawned;
    }

    protected override void OnDisable() {
        base.OnDisable();

        EnemyManager.GetInstanceFunc().onFirstEnemySpawned -= onFirstEnemySpawned;
    }


    public override void Attack() {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeBullet(bulletSpeed, damage, target.gameObject);
    }

    protected override IEnumerator FireCooldown(float wait) {
        List<Enemy> globalEnemies = EnemyManager.getEnemiesFunc.Invoke();


        //If there are enemies in range...
        if (globalEnemies.Count > 0) {
            SetTarget(globalEnemies);

            if (target != null) {
                Attack();
            }

            yield return new WaitForSeconds(wait);
            StartCoroutine(FireCooldown(internalAttackSpeed));
        }

        yield return null;
    }

    private void onFirstEnemySpawned() {
        StartCoroutine(FireCooldown(internalAttackSpeed));
    }
}