using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : ItemParent
{
    public GameObject bullets;
    public override void UseItem(Transform transform = null) 
    {
        GameObject gameoject = Instantiate(bullets, transform.forward + transform.position, transform.rotation);
        StartCoroutine(wait());
        
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(.25f);
        GameObject gameoject2 = Instantiate(bullets, transform.forward + transform.position, transform.rotation);
    }
}
