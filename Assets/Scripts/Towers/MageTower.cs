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
}