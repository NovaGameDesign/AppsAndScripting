using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStats : S_CharacterStats, IDamage
{

    private GameObject []player;
    
    private void Start()
    {
       
        player = GameObject.FindGameObjectsWithTag("Player");
    }
    private void Update()
    {
        if(healthbar != null && player != null) 
        {
            //Debug.Log("Turning!");
            var rotation = Quaternion.LookRotation(player[0].transform.position, gameObject.transform.position);
            var healthbarRotation = healthbar.transform.rotation;
            healthbarRotation.y = rotation.y;
            healthbar.transform.rotation = healthbarRotation;
        }
    }
    public void DealDamage(float incomingDamage)
    {
        health -= incomingDamage;
        //Debug.Log("The player's new health is: " + health+"/"+maxHealth);
        healthbar.value = health / maxHealth;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);

        }
    }
}
