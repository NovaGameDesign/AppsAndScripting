using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class S_CharacterStats : MonoBehaviour
{
    //Universal Stats 
    public float health;
    public float maxHealth = 100;
    public int regenAmount;
    public float regenDelay = 10;

    //Defense should always be a value
    /// <summary>
    /// Defense is the precent taken off damage, this value should never be greater than 1.
    /// In the event that it is greater than 1, damage will instead increase as we are multiplying <see cref="damage"/> * defense.
    /// </summary>
    public float defense;

    protected int ammo;
    protected int maxAmmo;

    public Slider healthbar;
    
    public bool damageable = true;

    public void increaseHealth(float healAmount)
    {
        health += healAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthbar.value = health / maxHealth;
    }

    public IEnumerator RegnerateHealth()
    { 
       
        yield return new WaitForSeconds(regenDelay);
        health += regenAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthbar.value = health / maxHealth;

        if(health == maxHealth)
        {
            StopAllCoroutines(); //Stop all coroutines once we reach max health. 
        }    
        else
            StartCoroutine(RegnerateHealth());
    }

    public void SetHealth(float healthValue)
    {
        health = healthValue;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthbar.value = health / maxHealth;
    }
}
