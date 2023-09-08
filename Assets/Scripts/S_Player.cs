using System;
using System.Collections;
using System.Collections.Generic;
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

    //Input System 
    private PlayerInput playerInput;
    public PlayerInputs playerControls;
    public int controlScheme;

    [System.NonSerialized] public InputAction move;
    [System.NonSerialized] public InputAction look;
    private InputAction fire;
   
    // ___________________________________________
    private void Awake()
    {
        controlScheme = 1;
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        rb = gameObject.GetComponent<Rigidbody>();

     
        move = playerInput.actions["Keyboard Movement Primary"];    
        look = playerInput.actions["Look Primary"];
        fire = playerInput.actions["Fire"];
        fire.performed += Fire;

        // Need to disable only the TFGH/WASD/Controller inputs of move rather than the entire thing. 
    }
    /*
     Old code from when I was using generate C# class rather than the input component itself. 
    private void OnEnable()
    {   
        move = playerControls.Player.Move;
        move.Enable();

        look = playerControls.Player.Look;
        look.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        move.Disable();
        look.Disable();
        fire.Disable();
    }*/

    // ___________________________________________

    

    void FixedUpdate()
    {
     
        //Camera Control
        lookDirection = look.ReadValue<Vector2>();
        playerRotation.x += lookDirection.x * mouseSensitivity * Time.deltaTime;
        playerRotation.y -= lookDirection.y * mouseSensitivity * Time.deltaTime;
        playerRotation.x = Mathf.Repeat(playerRotation.x, 360);
        playerRotation.y = Mathf.Clamp(playerRotation.y, -75, 75);
        rb.rotation = Quaternion.Euler(0f, playerRotation.x, 0f);
        mainCamera.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0f);
       

        
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
        Debug.Log("We Fired!");
    }
}