using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotInfo : MonoBehaviour
{
    //Core Information
    public TextMeshProUGUI[] itemNameInfo;
    public TextMeshProUGUI[] itemDescriptionInfo;
    public Image[] itemIconInfo;
    public TextMeshProUGUI itemQuantityInfo;
    public TextMeshProUGUI itemLoreNameInfo;
    public TextMeshProUGUI itemLoreDescriptionInfo;
    public GameObject itemLore;
    [System.NonSerialized] public int itemIndex;

 
}
