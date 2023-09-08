using System.Timers;
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

    private void Awake()
    {
        maxHealth = health;       
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
