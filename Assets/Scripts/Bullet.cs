using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public int damage;
    public int speed = 5;

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
        if (collision.gameObject.TryGetComponent<PlayerStats>(out var player))
        {
            if(player.damageable) 
            {
                player.SendMessage("DealDamage", damage);
            }
            Destroy(this.gameObject);
            
        }
        else if (collision.gameObject.TryGetComponent<EnemyStats>(out var enemy))
        {
            if (enemy.damageable)
            {
                enemy.SendMessage("DealDamage", damage);
            }
            Destroy(this.gameObject);
        }
        else
            Debug.Log("Something other than the player or enemy was hit.");
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5f);

        Destroy(this.gameObject);  
    }
}
