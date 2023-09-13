using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyBase : MonoBehaviour
{
    //[SerializeField] private string damageType = "Default";
    public bool damageable = true;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("DealDamage", 5);
        }
        else Debug.Log("Something other than the player entered the Enemy's Collider.");

    }
   
}
