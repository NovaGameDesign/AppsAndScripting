using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_PlayerStats : S_CharacterStats, IDamage
{

    [SerializeField] Slider healthbar;
    

    private void Awake()
    {
        healthbar.value = health;
    }

    public void DealDamage(float incomingDamage, string damageType)
    {
        // we could eventually add functionality for different damage types, ie. physical, plasma, fire, etc., allowing for bespoke logic when needed. Right now it isn't useful be eventually it could be. 
        if(damageType == "Physical")
        {
            incomingDamage = incomingDamage * defense;      
        }    

        health -= (int)incomingDamage;
        healthbar.value = health / maxHealth; 
        if(health <= 0)
        {
            //Death Logic 
        }
    }  


}


