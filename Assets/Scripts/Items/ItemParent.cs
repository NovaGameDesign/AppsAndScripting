using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

/// <summary>
/// Item type used to determined which slot it should fall under. Types include: Weapons, Armor, and Consumables. 
/// </summary>
public enum ItemType
{
    weapon,
    armor,
    consumable,
}


public class ItemParent : MonoBehaviour
{
    public int numOfItems = 0;
    /// <summary>
    /// What we use in code to detect if the item already exists in the inventory. 
    /// </summary>
    public string itemId = null;
    /// <summary>
    /// What we display to the player in game. 
    /// </summary>
    public string itemName = null;
    public string itemDescription = null;
    public string itemLore = null;

    public ItemType type = ItemType.consumable;
    public GameObject itemReference;
    [System.NonSerialized] public bool isActive = false;

    //Ui    
    public Sprite icon = null;


    public virtual void UseItem(Transform transform = null) { }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 100 * Time.deltaTime);
    }

}
