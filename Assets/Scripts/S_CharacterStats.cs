using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class S_CharacterStats : MonoBehaviour
{
    //Universal Stats 
    public int health;
    protected int maxHealth;
    public int regenRate;

    //Defense should always be a value
    public float defense;

    protected int ammo;
    protected int maxAmmo;

    private void Start()
    {
        maxHealth = health;
        System.Timers.Timer regenTimer = new System.Timers.Timer();
        regenTimer.Elapsed += new ElapsedEventHandler(regenerateHealth);
        regenTimer.Interval = 1000;
        regenTimer.Enabled = true;
    }

    private void reduceHealth(float incomingDamage)
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

}
