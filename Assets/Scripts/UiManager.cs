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

    [SerializeField] private TextMeshProUGUI controlSchemeText;
    [SerializeField] private GameObject menu;


    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        player = GetComponentInParent<S_Player>();
        controlSchemeText.text = "Primary Settings";

        openMenu = playerInput.actions["Open Menu"];
        openMenu.performed += openGameMenu;
        closeMenu = playerInput.actions["Close Menu"];
        closeMenu.performed += openGameMenu;       
    }

    void openGameMenu(InputAction.CallbackContext context)
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            playerInput.SwitchCurrentActionMap("Player");            
        }
        else
        {            
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
