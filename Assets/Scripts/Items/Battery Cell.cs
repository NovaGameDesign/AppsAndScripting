using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCell : ItemParent
{
    public PlayerStats stats;
    public override void UseItem(Transform transform)
    {
        stats.increaseHealth(10);
    }

}
