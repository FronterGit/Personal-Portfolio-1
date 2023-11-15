using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MageTower : Tower {
    public override void SetTarget(List<Enemy> enemies) {
        target = enemies[0].transform;
    }

    public override void Attack() {
        List<Enemy> attacked = new();
        for (var i = enemiesInRange.Count - 1; i >= 0; i--) {
            // Debug.Log("i: " + i + ", count: " + enemiesInRange.Count + ", frame: " + Time.frameCount);
            if (!attacked.Contains(enemiesInRange[i])) {
                attacked.Add(enemiesInRange[i]);
                enemiesInRange[i].TakeDamage(damage);
            }
        }
    }

    protected override IEnumerator FireCooldown(float wait) {
        //If there are enemies in range...
        if (enemiesInRange.Count > 0) {
            //...set the target to the first enemy in range,
            SetTarget(enemiesInRange);

            if (target != null) {
                Attack();
            }

            toRemoveAtEndOfFrame.ForEach(e => enemiesInRange.Remove(e));
            toRemoveAtEndOfFrame.Clear();

            //Wait for the fireRate before shooting again
            yield return new WaitForSeconds(wait);
            StartCoroutine(FireCooldown(internalAttackSpeed));
        } else yield return null;
    }

    protected override void OnTriggerExit2D(Collider2D collision) {
        //If an enemy leaves the tower's range, remove it from the list of enemies in range
        if (collision.CompareTag("Enemy")) {
            // enemiesInRange.Remove(collision.GetComponent<Enemy>());

            toRemoveAtEndOfFrame.Add(collision.GetComponent<Enemy>());

            //If there are no more enemies in range, stop shooting
            if (enemiesInRange.Count == 0) StopAllCoroutines();
        }
    }
}