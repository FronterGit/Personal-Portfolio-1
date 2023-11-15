using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : Enemy
{
    public override void TakeDamage(int damage)
    {
        //If this is the first time the enemy is hit, slow it down
        if (health == maxHealth) speed *= 0.5f;
        
        //Then take damage as normal
        base.TakeDamage(damage);
    }
}
