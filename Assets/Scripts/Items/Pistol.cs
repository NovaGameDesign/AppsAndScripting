using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : ItemParent
{
    public GameObject bullets;
    public int fireRate;

    private void Update()
    {
        
    }
    public override void UseItem(Transform transform)
    {
        if(canUse)
        {
            //Debug.Log("Attempted to shoot pistol");
            GameObject gameoject = Instantiate(bullets, transform.forward + transform.position, transform.rotation);
            itemSFX.Play();
            canUse = false;
            StartCoroutine(useDelay());
        }
      
    }

    public IEnumerator useDelay()
    {
        yield return new WaitForSeconds(timeDelay);
        canUse = true;
    }
}
