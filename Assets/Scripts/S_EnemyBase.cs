using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyBase : MonoBehaviour
{
    
    public bool damageable = true;
    public GameObject [] bullets;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("DealDamage", 5);
        }
        else Debug.Log("Something other than the player entered the Enemy's Collider.");

    }

    public void RangedAttack()
    {
        GameObject bullet = Instantiate(bullets[0].gameObject, (1.2f * transform.forward) + transform.position, transform.rotation);
    }

    public void MeleeAttack()
    {

    }
   
}
