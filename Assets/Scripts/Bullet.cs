using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public int damage;
    public int speed = 5;
    public bool hitsPlayer;
    public bool hitsAllies;
    public bool hitsEnemies;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyBullet());

    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitsPlayer)
        {
            if (collision.gameObject.TryGetComponent<PlayerStats>(out var player))
            {
                if (player.damageable)
                {
                    player.DealDamage(damage);
                }
                Destroy(this.gameObject);

            }
        }
        if (hitsAllies)
        {
            if (collision.gameObject.TryGetComponent<AllyStats>(out var enemy))
            {
                if (enemy.damageable)
                {
                    enemy.DealDamage(damage);
                }
                Destroy(this.gameObject);
            }
        }
       if (hitsEnemies)
        {
            if (collision.gameObject.TryGetComponent<EnemyStats>(out var enemy))
            {
                if (enemy.damageable)
                {
                    enemy.DealDamage(damage);
                }
                Destroy(this.gameObject);
            }
        }

        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5f);

        Destroy(this.gameObject);  
    }
}
