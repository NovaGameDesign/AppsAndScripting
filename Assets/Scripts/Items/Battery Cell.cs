using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCell : ItemParent
{

    public override void UseItem()
    {
        Debug.Log("Attempted to use the battery cell");
    }

}
