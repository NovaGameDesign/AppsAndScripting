using System.Collections;
using System.Timers;
using Unity.VisualScripting;
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

    private void reduceHealth(float incomingDamage) // we didn't end up using this at all, lol 
    {
        incomingDamage = incomingDamage * defense;
        health -= (int)incomingDamage;
    }

    private static void regenerateHealth(object source, ElapsedEventArgs e)
    {
       /* if(source == S_CharacterStats)
        {
            
        }
        Debug.Log("Health increased!");*/
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
}
