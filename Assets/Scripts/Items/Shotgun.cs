using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : ItemParent
{
    public GameObject bullets;
    public override void UseItem(Transform transform = null) 
    {
        
        for(int i = 0; i <6; i++) 
        {
            float randomSpotY = Random.Range(-1f, 1f);
            float randomSpotX = Random.Range(-3f, 3f);
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + randomSpotY, transform.position.z + randomSpotX);
            GameObject gameoject = Instantiate(bullets, spawnPos + (transform.forward * 2), transform.rotation);
        }
      
        
    }
}
