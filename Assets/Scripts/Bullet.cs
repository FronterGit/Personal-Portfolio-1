using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject target;

    public void InitializeBullet(float speed, int damage, GameObject target)
    {
        this.speed = speed;
        this.damage = damage;
        this.target = target;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, target.transform.position - transform.position);
    }

    private void Update()
    {
        if(target != null)
        {
            Move();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            OnHitEnemy();
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    private void OnHitEnemy()
    {
        EventBus<EnemyHitEvent>.Raise(new EnemyHitEvent(damage, target.GetComponent<Enemy>()));
    }
}
