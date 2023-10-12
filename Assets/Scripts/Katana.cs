using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    public float damage;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 100 * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
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
}
