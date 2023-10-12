using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : ItemParent
{
    public GameObject bullets;
    public override void UseItem(Transform _transform = null) 
    {
        GameObject gameoject = Instantiate(bullets, (2*_transform.forward) + _transform.position, _transform.rotation);
        //StartCoroutine(wait(_transform));
        
    }

    public IEnumerator wait(Transform _transform)
    {
        yield return new WaitForSeconds(.5f);
        GameObject gameoject2 = Instantiate(bullets, _transform.forward + _transform.position, _transform.rotation);
    }
}
