using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : Enemy
{
    protected override void Start()
    {
        health = maxHealth;
    }
}
