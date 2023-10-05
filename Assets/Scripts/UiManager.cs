using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
    private S_Player player;
    private PlayerInput playerInput;
    private InputAction openMenu;
    private InputAction closeMenu;
    private InputAction scroll;

    [SerializeField] private TextMeshProUGUI controlSchemeText;
    [SerializeField] private GameObject menu;

    [SerializeField] private playerInventory inventoryReference;

    int scrollLevel;

    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        player = GetComponentInParent<S_Player>();
        controlSchemeText.text = "Primary Settings";

        openMenu = playerInput.actions["Open Menu"];
        openMenu.performed += openGameMenu;
        closeMenu = playerInput.actions["Close Menu"];
        closeMenu.performed += openGameMenu;

        scroll = playerInput.actions["Scroll Wheel"];
        scroll.started += ScrollMenu;   

    }

    void ScrollMenu(InputAction.CallbackContext context)
    {
        var temp = scroll.ReadValue<Vector2>().y;
       
        if (temp > 0)
        {
            scrollLevel++;
        }
        else if(temp < 0) { scrollLevel--; }

        if (scrollLevel < 0) scrollLevel = 0;
        Debug.Log(scrollLevel);
        inventoryReference.RefreshDisplayedItems(scrollLevel);
    }
    void openGameMenu(InputAction.CallbackContext context)
    {
        if (menu.activeSelf)
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            playerInput.SwitchCurrentActionMap("Player");
           
        }
        else
        {
            Time.timeScale = 0;
            menu.SetActive(true);         
            playerInput.SwitchCurrentActionMap("UI");            
        }
        //Debug.Log("Escape Button pressed");
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
