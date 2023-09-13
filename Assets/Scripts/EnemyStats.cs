using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : S_CharacterStats, IDamage
{
    public void DealDamage(float incomingDamage)
    {
        health -= incomingDamage;
        //Debug.Log("The player's new health is: " + health+"/"+maxHealth);
        healthbar.value = health / maxHealth;
        if (health <= 0)
        {
            Destroy(this.gameObject);

        }
    }
}
