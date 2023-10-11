using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : ItemParent
{
    public GameObject bullets;
    public int fireRate;

    public override void UseItem(Transform transform)
    {
        //Debug.Log("Attempted to shoot pistol");
        GameObject gameoject = Instantiate(bullets, transform.forward + transform.position, transform.rotation);
    }
}
