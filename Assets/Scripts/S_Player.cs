using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class S_Player : MonoBehaviour
{
    //[Header("Movement")]
    Rigidbody rb;

    private Vector3 playerVelocity;
    private Vector3 playerRotation; //Camera
    [SerializeField] float playerSpeed = 2.0f;
    
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    public float mouseSensitivity = 1f;


    private Camera mainCamera;

    //Bullets
    public GameObject [] bullets;

    //Input System 
    private PlayerInput playerInput;
    public PlayerInputs playerControls;
    [System.NonSerialized] public int controlScheme;
    [System.NonSerialized] public InputAction move;
    [System.NonSerialized] public InputAction look;
    [System.NonSerialized] public InputAction fire;

    public playerInventory inventory;
   
    // ___________________________________________
    private void Awake()
    {
        controlScheme = 1;
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        rb = gameObject.GetComponent<Rigidbody>();
        inventory = gameObject.GetComponent<playerInventory>();

     
        move = playerInput.actions["Keyboard Movement Primary"];    
        look = playerInput.actions["Look Primary"];
        fire = playerInput.actions["Fire"];
        fire.performed += Fire;

        // Need to disable only the TFGH/WASD/Controller inputs of move rather than the entire thing. 
    }


    private void Update()
    {

        //Camera Control
        lookDirection = look.ReadValue<Vector2>();
        playerRotation.x += lookDirection.x * mouseSensitivity * Time.deltaTime;
        playerRotation.y -= lookDirection.y * mouseSensitivity * Time.deltaTime;
        playerRotation.x = Mathf.Repeat(playerRotation.x, 360f);
        playerRotation.y = Mathf.Clamp(playerRotation.y, -90f, 90f);
        rb.rotation = Quaternion.Euler(0f, playerRotation.x, 0f);
        mainCamera.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0f);
    }

    void FixedUpdate()
    {        
        //Movement
        playerVelocity = GetMovementInput();
        PlayerMove();
        //Vector3 movement = mainCamera.transform.right * moveDirection.x + mainCamera.transform.forward * moveDirection.y;
        rb.AddRelativeForce(playerVelocity);  
    }

    private Vector3 GetMovementInput()
    {
        moveDirection = move.ReadValue<Vector2>();
        return new Vector3(moveDirection.x, 0f, moveDirection.y);
    }
    private void PlayerMove()
    {        
        playerVelocity = (new Vector3(playerVelocity.x * playerSpeed, playerVelocity.y, playerVelocity.z * playerSpeed));
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (inventory.quickItems.ElementAt(0).isActive == true & inventory.quickItems.ElementAt(0).itemId != null & inventory.quickItems.ElementAt(0).type == ItemType.weapon)
        {
            inventory.quickItems.ElementAt(0).UseItem(transform);
        }
        else if (inventory.quickItems.ElementAt(1).isActive == true & inventory.quickItems.ElementAt(1).itemId != null & inventory.quickItems.ElementAt(1).type == ItemType.weapon)
        {
            inventory.quickItems.ElementAt(1).UseItem(transform);
      
        }
        else if (inventory.quickItems.ElementAt(2).isActive == true & inventory.quickItems.ElementAt(2).itemId != null & inventory.quickItems.ElementAt(2).type == ItemType.weapon)
        {
            inventory.quickItems.ElementAt(2).UseItem(transform);
       
        }
        else if (inventory.quickItems.ElementAt(3).isActive == true & inventory.quickItems.ElementAt(3).itemId != null & inventory.quickItems.ElementAt(3).type == ItemType.weapon)
        {
            inventory.quickItems.ElementAt(3).UseItem(transform);
        }
        else Debug.Log("No Item Selected");


        /*int i = UnityEngine.Random.Range(0, 2);
        if(i == 0) 
        {
           GameObject bullet = Instantiate(bullets[0], transform.forward + transform.position, transform.rotation);          
        }
        else
        {
            GameObject bullet = Instantiate(bullets[1], transform.forward + transform.position, transform.rotation);
        }
        */
    }
}