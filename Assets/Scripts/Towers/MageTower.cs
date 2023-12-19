using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MageTower : Tower
{
    [SerializeField] private List<Enemy> enemiesToRemove;
    private bool attacking = false;
    public override void SetTarget(List<Enemy> enemies) {
        //Do nothing
    }

    public override void Attack() {
        //Set attacking to true
        attacking = true;
        
        //Damage all enemies in range
        foreach(Enemy e in enemiesInRange) {
            
            if(e != null) e.TakeDamage(damage);
            
        }
        
        //Set attacking to false
        attacking = false;
        
        //Remove all enemies that left the range during the attack
        enemiesToRemove.ForEach(e => enemiesInRange.Remove(e));
    }

    protected override IEnumerator FireCooldown(float wait, float delay = 0f) 
    {
        //If there is a delay, wait for the delay before shooting
        if (delay > 0) yield return new WaitForSeconds(delay);
        
        //If there are enemies in range...
        if (enemiesInRange.Count > 0) {
            //Remember the time when we attack
            attackedTime = Time.time;
            
            //Call the attack method
            Attack();
            
            //Wait for the fireRate before shooting again
            yield return new WaitForSeconds(wait);
            StartCoroutine(FireCooldown(internalAttackSpeed));
        } else yield return null;
    }


    protected override void OnTriggerExit2D(Collider2D collision) {
        //If an enemy leaves the tower's range, remove it from the list of enemies in range
        if (collision.CompareTag("Enemy")) {
            //If we are not attacking, we can remove the enemies
            if (!attacking) enemiesInRange.Remove(collision.GetComponent<Enemy>());
            //Else, remember them and remove them at the end of the attack. We do this because we can't remove them while iterating over the list.
            else enemiesToRemove.Add(collision.GetComponent<Enemy>());
            
            //If there are no more enemies in range, stop shooting
            if (enemiesInRange.Count == 0) StopAllCoroutines();
        }
    }
}