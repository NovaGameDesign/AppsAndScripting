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
        //if (collision.gameObject == gameObject.Player)
        //{
            
       // }
        Debug.Log("Entered the enemy's collider");

    }
   
}
