using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : S_CharacterStats, IDamage
{  

    private void Awake()
    {
        healthbar.value = health / maxHealth;
    }

    public void DealDamage(float incomingDamage)
    {
        StopAllCoroutines();
        //Debug.Log("Player took damage, their current health is: " + health);
        health -= incomingDamage;
        //Debug.Log("The player's new health is: " + health+"/"+maxHealth);
        healthbar.value = health / maxHealth;        
        if(health <= 0)
        {
            //Death Logic 
            
        }
        StartCoroutine(RegnerateHealth());
    }  


}


