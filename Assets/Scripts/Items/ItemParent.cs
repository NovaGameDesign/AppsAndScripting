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
    [Header("Item Info")]
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

    public AudioSource itemSFX = null;
    public bool inInventory = false;
    public float timeDelay;
    [System.NonSerialized] public bool canUse = true;
    public virtual void UseItem(Transform transform = null) { }
   
    private void Awake()
    {
        /*if(icon == null)
        {
            Debug.Log("Icon was null, loading the null PNG from assets");
            icon = Resources.Load<Sprite>(Application.dataPath + "Assets/materials/null.png");
        }*/
    }
    private void Update()
    {
        if(!inInventory)
        {
            transform.RotateAround(transform.position, Vector3.up, 100 * Time.deltaTime);
        }
        
    }
}
