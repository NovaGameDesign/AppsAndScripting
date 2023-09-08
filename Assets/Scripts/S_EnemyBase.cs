using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyBase : MonoBehaviour
{
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Collider");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("The Player entered the enemy's collider");
        }
        else Debug.Log("Something other than the player entered the Enemy's Collider.");

    }
   
}
