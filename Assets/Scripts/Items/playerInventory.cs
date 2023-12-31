using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class playerInventory : MonoBehaviour
{  

    public int numOfHoldableItems = 12;
    [System.NonSerialized] public LinkedList<ItemParent> items = new LinkedList<ItemParent>();
    [System.NonSerialized] public LinkedList<ItemParent> quickItems = new LinkedList<ItemParent>();
    [SerializeField] private ItemParent emptyItem;

    //The Ui Part 
    protected List<GameObject> InventoryUi = new List<GameObject>();
    /// <summary>
    /// The quick item slots that appear in game, not the ones in the menu. 
    /// </summary>
    protected List<GameObject> QuickItemSlots = new List<GameObject>();
    /// <summary>
    /// Menu version of the quick item slots 
    /// </summary>
    [SerializeField] private List<GameObject> QuickItemSlotsMenu = new List<GameObject>();
    /// <summary>
    /// The border that appears around the currently selected item.
    /// </summary>
    protected List<GameObject> QuickItemBorder = new List<GameObject>();
    protected GameObject itemOut;
    public Transform itemSpawnPoint;

    public ItemSlotInfo[] itemSlotsUI;
    private int slotId = -1;
    [SerializeField]private GameObject[] pages;

    private GameObject errorMessage;

    int scrollLevel;

    //Inputs
    private S_Player player;
    private InputAction useitem; private InputAction item1; private InputAction item2; private InputAction item3; private InputAction item4;
    private PlayerInput playerInput;
    private InputAction openMenu;
    private InputAction closeMenu;
    private InputAction scroll;


    [SerializeField] private TextMeshProUGUI controlSchemeText;
    [SerializeField] private GameObject menu;

    int UILayer;

    [SerializeField] ItemParent[] blankQuickitems;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        player = GetComponentInParent<S_Player>();
        controlSchemeText.text = "Primary Settings";

        openMenu = playerInput.actions["Open Menu"];
        openMenu.started += openGameMenu;
        closeMenu = playerInput.actions["Close Menu"];
        closeMenu.started += openGameMenu;

        scroll = playerInput.actions["Scroll Wheel"];
        scroll.performed += ScrollMenu;

        useitem = playerInput.actions["Use Item"]; item1 = playerInput.actions["Item 1"]; item2 = playerInput.actions["Item 2"]; item3 = playerInput.actions["Item 3"]; item4 = playerInput.actions["Item 4"];
        useitem.performed += UseItem;
        item1.performed += SwapActiveQuickItem; item2.performed += SwapActiveQuickItem; item3.performed += SwapActiveQuickItem; item4.performed += SwapActiveQuickItem;
    }

    // On Enable and Disable handles the input related functionality. In short, we just need to make sure that when input is no longer allowed, we actually "turn off" the recievers. 
    private void OnEnable()
    {
        openMenu.Enable();
        closeMenu.Enable();
        scroll.Enable();
        useitem.Enable();
        item1.Enable();
        item2.Enable();
        item3.Enable();
        item4.Enable();
    }
    private void OnDisable()
    {
        openMenu.Disable();
        closeMenu.Disable();
        scroll.Disable();
        useitem.Disable();
        item1.Disable();
        item2.Disable();
        item3.Disable();
        item4.Disable();
    }


    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        errorMessage = GameObject.Find("Error Message");
        errorMessage.SetActive(false);


        for (int i = 0; i < 4; i++)
        {
            quickItems.AddFirst(blankQuickitems[i]);
        }

        for(int i = 0; i < 4; i++)
        {
            QuickItemSlots.Add(GameObject.Find("Q"+(i+1)));
            QuickItemBorder.Add(QuickItemSlots[i].GetComponentsInChildren<Image>()[2].gameObject);
            QuickItemBorder[i].SetActive(false);
        }

    }

    private void Update()
    {
        //Debug.Log(IsPointerOverUIElement() ? "Over UI" : "Not over UI"); Debug version for testing, same function just prints to log 
        IsPointerOverUIElement();   
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    /// <summary>
    /// Returns true if we were hovering over a UI element. Additionally, functionality to hide/show different lore pages based on what the user is currently hovering over. 
    /// </summary>
    /// <param name="eventSystemRaysastResults"></param>
    /// <returns></returns>
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
            {
                switch (curRaysastResult.gameObject.name)
                {
                    case "Item 1":
                        {
                            itemSlotsUI[0].lorePage.SetActive(true); itemSlotsUI[1].lorePage.SetActive(false); itemSlotsUI[2].lorePage.SetActive(false); itemSlotsUI[3].lorePage.SetActive(false); itemSlotsUI[4].lorePage.SetActive(false); itemSlotsUI[5].lorePage.SetActive(false);
                            break;
                        }
                    case "Item 2":
                        {
                            itemSlotsUI[0].lorePage.SetActive(false); itemSlotsUI[1].lorePage.SetActive(true); itemSlotsUI[2].lorePage.SetActive(false); itemSlotsUI[3].lorePage.SetActive(false); itemSlotsUI[4].lorePage.SetActive(false); itemSlotsUI[5].lorePage.SetActive(false);
                            break;
                        }
                    case "Item 3":
                        {
                            itemSlotsUI[0].lorePage.SetActive(false); itemSlotsUI[1].lorePage.SetActive(false); itemSlotsUI[2].lorePage.SetActive(true); itemSlotsUI[3].lorePage.SetActive(false); itemSlotsUI[4].lorePage.SetActive(false); itemSlotsUI[5].lorePage.SetActive(false);
                            break;
                        }
                    case "Item 4":
                        {
                            itemSlotsUI[0].lorePage.SetActive(false); itemSlotsUI[1].lorePage.SetActive(false); itemSlotsUI[2].lorePage.SetActive(false); itemSlotsUI[3].lorePage.SetActive(true); itemSlotsUI[4].lorePage.SetActive(false); itemSlotsUI[5].lorePage.SetActive(false);
                            break;
                        }
                    case "Item 5":
                        {
                            itemSlotsUI[0].lorePage.SetActive(false); itemSlotsUI[1].lorePage.SetActive(false); itemSlotsUI[2].lorePage.SetActive(false); itemSlotsUI[3].lorePage.SetActive(false); itemSlotsUI[4].lorePage.SetActive(true); itemSlotsUI[5].lorePage.SetActive(false);
                            break;
                        }
                    case "Item 6":
                        {
                            itemSlotsUI[0].lorePage.SetActive(false); itemSlotsUI[1].lorePage.SetActive(false); itemSlotsUI[2].lorePage.SetActive(false); itemSlotsUI[3].lorePage.SetActive(false); itemSlotsUI[4].lorePage.SetActive(false); itemSlotsUI[5].lorePage.SetActive(true);
                            break;
                        }
                    default:
                        {
                            // Debug.Log("there was no valid UI element to display a lore page for. The UI element's name was: " + curRaysastResult.gameObject.name);
                            break;
                        }
                }
               
                return true;
            }
                
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    public void SetSlotId(int SlotId)
    {
        
        slotId = itemSlotsUI[SlotId].itemIndex;
        Debug.Log("The Desired slot index was: " + slotId);
    }

    public void StartQuickItemSwap(int quickItemSlot)
    {
        slotId = -1;
        RefreshDisplayedItems(0);
        pages[0].SetActive(false);
        pages[1].SetActive(true);

        StartCoroutine(waitForNewItem(quickItemSlot));      
    }
    private IEnumerator waitForNewItem(int quickItemSlot)
    {
        while (slotId == -1)
        {
            Debug.Log("Waiting on item selection");
            yield return new WaitUntil(() => slotId >= 0);
        }

        pages[0].SetActive(true);
        pages[1].SetActive(false);
        quickItems.Find(quickItems.ElementAt(quickItemSlot)).Value = items.ElementAt(slotId); //This is so messy please tell me there is a better way to do this... AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHH
        QuickItemSlotsMenu[quickItemSlot].GetComponent<Image>().sprite = items.ElementAt(slotId).icon;
        UpdateItemDisplay(quickItemSlot);
    }

    public void RefreshDisplayedItems(int startingIndex)
    {
        if(startingIndex + 6 > items.Count)
        {
            startingIndex = items.Count - 6;           
        }
        if (startingIndex < 0)
        {
            startingIndex = 0;
        }

        int index = startingIndex;
        for(int i = 0; i < 6;  i++) // Loop six times, or the amount of slots we can display at one time. 
        {          
            if(index >= items.Count)
            {
                //Debug.Log("We have less items than we can display. Canceling the For loop");
                ItemParent empty = new ItemParent();
                itemSlotsUI[i].itemIconInfo[0].sprite = empty.icon;
                itemSlotsUI[i].itemIconInfo[1].sprite = empty.icon;
                itemSlotsUI[i].itemIconInfo[0].color = Color.clear;
                itemSlotsUI[i].itemIconInfo[1].color = Color.clear;
                //Save Index 
                itemSlotsUI[i].itemIndex = index;
                //Update Name
                itemSlotsUI[i].itemNameInfo[0].text = " ";
                itemSlotsUI[i].itemNameInfo[1].text = " ";
                //Update Description
                itemSlotsUI[i].itemDescriptionInfo[0].text = " ";
                itemSlotsUI[i].itemDescriptionInfo[1].text = " ";
                //Quantity
                itemSlotsUI[i].itemQuantityInfo.text = " ";
                //LORE! Allegedly 
                itemSlotsUI[i].itemLoreDescriptionInfo.text = " ";
             
            }
            else
            {
                //Debug.Log("The items we now have are: " +items.ElementAt(index).itemName);
                //Update Icons
                itemSlotsUI[i].itemIconInfo[0].sprite = items.ElementAt(index).icon;
                itemSlotsUI[i].itemIconInfo[1].sprite = items.ElementAt(index).icon;
                itemSlotsUI[i].itemIconInfo[0].color = new Color(255, 255, 255, 255);
                itemSlotsUI[i].itemIconInfo[1].color = new Color(255, 255, 255, 255);
                //Save Index 
                itemSlotsUI[i].itemIndex = index;
                //Update Name
                itemSlotsUI[i].itemNameInfo[0].text = items.ElementAt(index).itemName;
                itemSlotsUI[i].itemNameInfo[1].text = items.ElementAt(index).itemName;
                //Update Description
                itemSlotsUI[i].itemDescriptionInfo[0].text = items.ElementAt(index).itemDescription;
                itemSlotsUI[i].itemDescriptionInfo[1].text = items.ElementAt(index).itemDescription;
                //Quantity
                itemSlotsUI[i].itemQuantityInfo.text = items.ElementAt(index).numOfItems.ToString();
                //LORE! Allegedly 
                itemSlotsUI[i].itemLoreDescriptionInfo.text = items.ElementAt(index).itemLore;

                //Rest of the lore related functionality will come when I have more time. Should be simple to setup.
            }


            index++;
        }
    }

    /// <summary>
    /// Using the player's scroll wheel they are able to iterate through the inventory menu. 
    /// </summary>
    /// <param name="context"></param>
    void ScrollMenu(InputAction.CallbackContext context)
    {

        var temp = scroll.ReadValue<Vector2>().y;
        //Check if we should increase or decrease the value
        if (temp > 0)
        {
            scrollLevel--;
        }
        else if (temp < 0) 
        { 
            scrollLevel++;
        }

        //Check if we are trying to go past 0 or above the amount of items we have. 
        if (scrollLevel < 0)
        {
            scrollLevel = 0;
        }
        else if (scrollLevel > items.Count)
        {
            scrollLevel = items.Count;
        }

        //Debug.Log(scrollLevel);
        RefreshDisplayedItems(scrollLevel);
    }

    
    private void UseItem(InputAction.CallbackContext context)
    {
        if (quickItems.ElementAt(0).isActive == true & quickItems.ElementAt(0).itemId != null & quickItems.ElementAt(0).type == ItemType.consumable) 
        { 
            quickItems.ElementAt(0).UseItem();
            quickItems.ElementAt(0).numOfItems--;
            if (quickItems.ElementAt(0).numOfItems <= 0)
            {
                items.Remove(quickItems.ElementAt(0));
                var temp = quickItems.First;
                quickItems.ElementAt(0).gameObject.SetActive(false);
                quickItems.AddAfter(temp, blankQuickitems[0]);
                quickItems.Remove(quickItems.ElementAt(0));                
                QuickItemSlotsMenu[0].GetComponent<Image>().sprite = emptyItem.icon;
                QuickItemSlots[0].gameObject.GetComponentsInChildren<Image>()[1].sprite = emptyItem.icon;
                QuickItemSlots[0].gameObject.GetComponentInChildren<Text>().text = emptyItem.numOfItems.ToString();
            }
            else
            {
                UpdateItemDisplay(0);
            }
        }
        else if (quickItems.ElementAt(1).isActive == true & quickItems.ElementAt(1).itemId != null & quickItems.ElementAt(1).type == ItemType.consumable)
        {
            quickItems.ElementAt(1).UseItem();
            quickItems.ElementAt(1).numOfItems--;
            if (quickItems.ElementAt(1).numOfItems <= 0)
            {
                quickItems.ElementAt(1).gameObject.SetActive(false);
                items.Remove(quickItems.ElementAt(1));
                var temp = quickItems.First;
                temp = temp.Next;
                quickItems.AddAfter(temp, blankQuickitems[1]);
                quickItems.Remove(quickItems.ElementAt(1));
                QuickItemSlotsMenu[1].GetComponent<Image>().sprite = emptyItem.icon;
                QuickItemSlots[1].gameObject.GetComponentsInChildren<Image>()[1].sprite = emptyItem.icon;
                QuickItemSlots[1].gameObject.GetComponentInChildren<Text>().text = emptyItem.numOfItems.ToString();
            }
            else
            {
                UpdateItemDisplay(1);
            }
        }           
        else if (quickItems.ElementAt(2).isActive == true & quickItems.ElementAt(2).itemId != null & quickItems.ElementAt(2).type == ItemType.consumable)
        {
            quickItems.ElementAt(2).UseItem();
            quickItems.ElementAt(2).numOfItems--;
            if (quickItems.ElementAt(2).numOfItems <= 0)
            {
                quickItems.ElementAt(2).gameObject.SetActive(false);
                items.Remove(quickItems.ElementAt(2));
                var temp = quickItems.First;
                temp = temp.Next;
                temp = temp.Next;
                quickItems.AddAfter(temp, blankQuickitems[2]);
                quickItems.Remove(quickItems.ElementAt(2));
                QuickItemSlotsMenu[2].GetComponent<Image>().sprite = emptyItem.icon;
                QuickItemSlots[2].gameObject.GetComponentsInChildren<Image>()[1].sprite = emptyItem.icon;
                QuickItemSlots[2].gameObject.GetComponentInChildren<Text>().text = emptyItem.numOfItems.ToString();
            }
            else
            {
                UpdateItemDisplay(2);
            }
        
        }
        else if (quickItems.ElementAt(3).isActive == true & quickItems.ElementAt(3).itemId != null & quickItems.ElementAt(3).type == ItemType.consumable)
        {
            quickItems.ElementAt(3).UseItem();
            quickItems.ElementAt(3).numOfItems--;
            if (quickItems.ElementAt(3).numOfItems <= 0)
            {
                quickItems.ElementAt(3).gameObject.SetActive(false);
                items.Remove(quickItems.ElementAt(3));
                var temp = quickItems.First;
                temp = temp.Next;
                temp = temp.Next;
                temp = temp.Next;
                quickItems.AddAfter(temp, blankQuickitems[3]);
                quickItems.Remove(quickItems.ElementAt(3));
                QuickItemSlotsMenu[3].GetComponent<Image>().sprite = emptyItem.icon;
                QuickItemSlots[3].gameObject.GetComponentsInChildren<Image>()[1].sprite = emptyItem.icon;
                QuickItemSlots[3].gameObject.GetComponentInChildren<Text>().text = emptyItem.numOfItems.ToString();
            }
            else
            {
                UpdateItemDisplay(3);
            }
        }
        else Debug.Log("No Item Selected");
    }

    /// <summary>
    /// Right now we set canUse to true after swapping which means you can bypass the shoot/item use delay. 
    /// This isn't a big concern for this project, but in the future it'd be best to investigate a better solution Additionally manually setting each object to false or true depending on the context is a bit strange, it would also be better to find a different method there as well. Perhaps storing the last active index and then updating would be a better method. 
    /// </summary>
    /// <param name="context"></param>
    private void SwapActiveQuickItem(InputAction.CallbackContext context)
    {
        //Destroy(itemOut?.gameObject);
        if (context.action == item1)
        {

            quickItems.ElementAt(0).canUse = true;
            quickItems.ElementAt(0).isActive = true; quickItems.ElementAt(1).isActive = false; quickItems.ElementAt(2).isActive = false; quickItems.ElementAt(3).isActive = false;
            quickItems.ElementAt(0).gameObject.SetActive(true); quickItems.ElementAt(1).gameObject.SetActive(false); quickItems.ElementAt(2).gameObject.SetActive(false); quickItems.ElementAt(3).gameObject.SetActive(false);
            QuickItemBorder[0].SetActive(true); QuickItemBorder[1].SetActive(false); QuickItemBorder[2].SetActive(false);QuickItemBorder[3].SetActive(false);            
            //itemOut = Instantiate(quickItems.ElementAt(0).itemReference, itemSpawnPoint.position, itemSpawnPoint.rotation);
        }
        else if (context.action == item2)
        {
            quickItems.ElementAt(1).canUse = true;
            quickItems.ElementAt(0).isActive = false; quickItems.ElementAt(1).isActive = true; quickItems.ElementAt(2).isActive = false; quickItems.ElementAt(3).isActive = false;
            quickItems.ElementAt(0).gameObject.SetActive(false); quickItems.ElementAt(1).gameObject.SetActive(true); quickItems.ElementAt(2).gameObject.SetActive(false); quickItems.ElementAt(3).gameObject.SetActive(false);
            QuickItemBorder[0].SetActive(false);QuickItemBorder[1].SetActive(true); QuickItemBorder[2].SetActive(false); QuickItemBorder[3].SetActive(false);
        }
        else if (context.action == item3)
        {
            quickItems.ElementAt(2).canUse = true;
            quickItems.ElementAt(0).isActive = false; quickItems.ElementAt(1).isActive = false; quickItems.ElementAt(2).isActive = true; quickItems.ElementAt(3).isActive = false;
            quickItems.ElementAt(0).gameObject.SetActive(false); quickItems.ElementAt(1).gameObject.SetActive(false); quickItems.ElementAt(2).gameObject.SetActive(true); quickItems.ElementAt(3).gameObject.SetActive(false);
            QuickItemBorder[0].SetActive(false); QuickItemBorder[1].SetActive(false); QuickItemBorder[2].SetActive(true);  QuickItemBorder[3].SetActive(false);
        }
        else if (context.action == item4)
        {
            quickItems.ElementAt(3).canUse = true;
            quickItems.ElementAt(0).isActive = false; quickItems.ElementAt(1).isActive = false; quickItems.ElementAt(2).isActive = false; quickItems.ElementAt(3).isActive = true;
            quickItems.ElementAt(0).gameObject.SetActive(false); quickItems.ElementAt(1).gameObject.SetActive(false); quickItems.ElementAt(2).gameObject.SetActive(false); quickItems.ElementAt(3).gameObject.SetActive(true);
            QuickItemBorder[0].SetActive(false); QuickItemBorder[1].SetActive(false); QuickItemBorder[2].SetActive(false);  QuickItemBorder[3].SetActive(true);
        }
    }

    /// <summary>
    /// Method that updates the Quick Item UI to match what we actually have in the inventory. 
    /// </summary>
    public void UpdateItemDisplay(int slot)
    {
   /*     //While a for loop would make this code cleaner, I ran into an issue with them I wasn't able to figure out. So now I just manually do all the setting... 
        for(int i = 0; i < 3; i++)
        {
            if(quickItems.ElementAt(i).icon != null)
            {
                QuickItemSlots[0].gameObject.GetComponentsInChildren<Image>()[1].sprite = quickItems.ElementAt(i)?.icon;
            }
            if(quickItems.ElementAt(i).numOfItems > 0)
            {
                QuickItemSlots[0].gameObject.GetComponentInChildren<Text>().text = quickItems.ElementAt(i)?.numOfItems.ToString();
            }         
           
        }*/

        QuickItemSlots[slot].gameObject.GetComponentsInChildren<Image>()[1].sprite = quickItems.ElementAt(slot)?.icon;
        QuickItemSlots[slot].gameObject.GetComponentInChildren<Text>().text = quickItems.ElementAt(slot)?.numOfItems.ToString();

      /*  QuickItemSlots[1].gameObject.GetComponentsInChildren<Image>()[1].sprite = quickItems.ElementAt(1)?.icon;
        QuickItemSlots[1].gameObject.GetComponentInChildren<Text>().text = quickItems.ElementAt(1)?.numOfItems.ToString();
        QuickItemSlots[2].gameObject.GetComponentsInChildren<Image>()[1].sprite = quickItems.ElementAt(1)?.icon;
        QuickItemSlots[2].gameObject.GetComponentInChildren<Text>().text = quickItems.ElementAt(2)?.numOfItems.ToString();
        QuickItemSlots[3].gameObject.GetComponentsInChildren<Image>()[1].sprite = quickItems.ElementAt(1)?.icon;
        QuickItemSlots[3].gameObject.GetComponentInChildren<Text>().text = quickItems.ElementAt(3)?.numOfItems.ToString();*/

    }


    public IEnumerator WaitFiveSeconds(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        errorMessage.SetActive(false);
    }

    /// <summary>
    /// When the player collides with an object we check whether it has the ItemScript. If it does, then we proceed to check whether it is already in the inventory
    /// We do this by grabbing the internal ID from the item script and using the TryGetValue method assosiated with the Dictionary variable type. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        GameObject item = collision.gameObject;               

        if(item.GetComponent<ItemParent>() != null) //checks if the thing we ran into was actually an item or not 
        {
            ItemParent itemScript = item.GetComponent<ItemParent>();
            if(items.Count >= 12)
            {
                errorMessage.GetComponent<TextMeshProUGUI>().text = "Inventory Full, free up a slot for this item to be added.";
                errorMessage.SetActive(true);
                StartCoroutine(WaitFiveSeconds(5));                
                return;
            }

            if(items.Count <= 0)
            {               
                items.AddFirst(itemScript);
                // Debug.Log("There was no items in the Linked List, so we added one."+items);
                itemScript.inInventory = true;
                collision.gameObject.transform.parent = Camera.main.transform;
                collision.gameObject.transform.position = itemSpawnPoint.position;
                collision.gameObject.transform.rotation = itemSpawnPoint.rotation;
                collision.gameObject.GetComponent<BoxCollider>().enabled = false;
                collision.gameObject.SetActive(false);                
                //collision.gameObject.SetActive(false);
            } 
            else
            {             
                for (int i = 0; i < items.Count; i++)
                {
                    if (items.ElementAt(i).itemId == itemScript.itemId)
                    {
                        items.ElementAt(i).numOfItems += itemScript.numOfItems;
                        // Debug.Log("The item already existed! There is now: " + items.ElementAt(i).numOfItems);
                        collision.gameObject.SetActive(false);

                        break;
                    }
                    
                    if (items.Count == i+1) // the final iteration
                    {
                        items.AddLast(itemScript);
                        itemScript.inInventory = true;
                        collision.gameObject.transform.parent = Camera.main.transform;
                        collision.gameObject.transform.position = itemSpawnPoint.position;
                        collision.gameObject.transform.rotation = itemSpawnPoint.rotation;
                        collision.gameObject.GetComponent<BoxCollider>().enabled = false;
                        //   Debug.Log("The item didn't exist at all! We added it and there is now: " + items.Find(itemScript).Value.numOfItems);
                        collision.gameObject.SetActive(false);

                    }
                }
            }
        }
        else
        {
           // Debug.Log("The item didn't have the Item Tag, or it didn't have the item script/a child of it");
        }    
    }
    void openGameMenu(InputAction.CallbackContext context)
    {  
        if (menu.activeSelf)
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            playerInput.SwitchCurrentActionMap("Player");
            Debug.Log(playerInput.currentActionMap);

        }
        else
        {
            Time.timeScale = 0;
            menu.SetActive(true);
            RefreshDisplayedItems(0);
            playerInput.SwitchCurrentActionMap("UI");
            Debug.Log(playerInput.currentActionMap);
        }
       
    }

    public void ChangeControlScheme()
    {
        player.controlScheme++;

        if (player.controlScheme == 2)
        {
            player.move = playerInput.actions["Keyboard Movement Secondary"];
            player.look = playerInput.actions["Look Secondary"];
            controlSchemeText.text = "Secondary Settings";
            //Debug.Log("Settings Changed to Secondary");
        }
        else if (player.controlScheme == 3)
        {
            player.move = playerInput.actions["Controller Movement"];
            player.look = playerInput.actions["Look Controller"];
            controlSchemeText.text = "Controller Settings";
            //Debug.Log("Settings Changed to Controller");
        }
        else
        {
            player.controlScheme = 1;
            player.move = playerInput.actions["Keyboard Movement Primary"];
            player.look = playerInput.actions["Look Primary"];
            controlSchemeText.text = "Primary Settings";
        }
    }

}
