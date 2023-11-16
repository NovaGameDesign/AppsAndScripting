using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
    [Header("Position and Rotation")]
    public float playerPositonX;
    public float playerPositonY;
    public float playerPositonZ;
    public float playerRotationX;
    public float playerRotationY;
    public float playerRotationZ;
    [Header("Health and Inventory")]
    public float playerHealth;

    public LinkedList<ItemParent> items = new LinkedList<ItemParent>();
    public LinkedList<ItemParent> quickItems = new LinkedList<ItemParent>();

    [Header("Enemy Information")]
    public List<float> enemyPositionX = new List<float>();
    public List<float> enemyPositionY = new List<float>();
    public List<float> enemyPositionZ = new List<float>();
    public List<int> enemyId = new List<int>();
    public List<bool> enemyStillAlive = new List<bool>();
}
