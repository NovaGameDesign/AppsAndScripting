using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : ItemParent
{
    public GameObject bullets;
    public override void UseItem(Transform _transform = null) 
    {
        if(canUse)
        {
            GameObject gameoject = Instantiate(bullets, (2 * _transform.forward) + _transform.position, _transform.rotation);
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
