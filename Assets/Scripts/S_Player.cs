using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class S_Player : MonoBehaviour
{
    [Header("Movement Related")]
    Rigidbody rb;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private Vector3 playerVelocity;
    private Vector3 playerRotation;
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] float stepSmooth = .1f;
    [SerializeField] float CheckDistance;
    [SerializeField] LayerMask layerMask;

    [Header("Jump Related")]
    [SerializeField] float jumpForce;
    [SerializeField] float maxJumpSpeed;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float fallSpeed;
    [SerializeField] float gravityMultiplier;
    bool grounded;
    float originalMass;
    bool jumping;


    [Header("Inventory Related")]
    public GameObject [] bullets;

    //Input System 
    private PlayerInput playerInput;
    public PlayerInputs playerControls;
    [System.NonSerialized] public int controlScheme;
    [System.NonSerialized] public InputAction move;
    [System.NonSerialized] public InputAction look;
    [System.NonSerialized] public InputAction fire;
    [System.NonSerialized] public InputAction jump;

    public playerInventory inventory;


    private Camera mainCamera;

    // ___________________________________________
    private void Awake()
    {
        controlScheme = 1;
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        rb = gameObject.GetComponent<Rigidbody>();
        originalMass = rb.mass;
        inventory = gameObject.GetComponent<playerInventory>();

     
        move = playerInput.actions["Keyboard Movement Primary"];    
        look = playerInput.actions["Look Primary"];
        fire = playerInput.actions["Fire"];
        jump = playerInput.actions["Jump"];
        fire.performed += Fire;
        jump.performed += CheckForJump;

        // Need to disable only the TFGH/WASD/Controller inputs of move rather than the entire thing. 
    }

    private void OnEnable()
    {
        move.Enable();
        look.Enable();
        fire.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        fire.Disable();
    }

    private void Update()
    {
        PlayerMove();
        //GroundCheck();

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
        rb.AddRelativeForce(playerVelocity);
        PlayerStep();
        //Jumping();
    }

    private void PlayerMove()
    {
        moveDirection = move.ReadValue<Vector2>();      
        playerVelocity = new Vector3(moveDirection.x, 0f, moveDirection.y);     
        playerVelocity = (new Vector3(playerVelocity.x * playerSpeed, playerVelocity.y, playerVelocity.z * playerSpeed));
    }

    private void PlayerStep()
    {
        if (moveDirection != Vector2.zero)
        {
            RaycastHit lowerHit;
            if (Physics.Raycast(stepRayLower.transform.position, stepRayLower.transform.position + (stepRayLower.transform.forward * moveDirection.y) + (stepRayLower.transform.right * moveDirection.x), out lowerHit, CheckDistance, layerMask))
            {
                Debug.Log(lowerHit.collider.gameObject.name);           
                Debug.Log("Lower Foot hit something");
                RaycastHit upperHit;
                if (!Physics.Raycast(stepRayUpper.transform.position, stepRayUpper.transform.position + (stepRayUpper.transform.forward * moveDirection.y) + (stepRayUpper.transform.right * moveDirection.x), out upperHit, 1, layerMask))
                {
                    rb.position -= new Vector3(0f, -stepSmooth, 0f);
                    Debug.Log("Start stepping");
                }
                else if(Physics.Raycast(stepRayUpper.transform.position, stepRayUpper.transform.position + (stepRayUpper.transform.forward * moveDirection.y) + (stepRayUpper.transform.right * moveDirection.x), out upperHit, CheckDistance, layerMask))
                {
                    Debug.Log(upperHit.collider.gameObject.name);
                    Debug.Log("The upper step did hit something.");
                }
            }
            Debug.DrawLine(stepRayLower.transform.position, stepRayLower.transform.position + (stepRayLower.transform.forward * moveDirection.y) + (stepRayLower.transform.right * moveDirection.x), Color.red, .1f);
            Debug.DrawLine(stepRayUpper.transform.position, stepRayUpper.transform.position + (stepRayUpper.transform.forward * moveDirection.y) + (stepRayUpper.transform.right * moveDirection.x), Color.blue, .2f);
        }       
    }

    private void CheckForJump(InputAction.CallbackContext context)
    {       
        if (!grounded)
        {
            return;
        }
        jumping = true;
        rb.mass = originalMass;
       
    }
    private void Jumping()
    {
        if(jumping)
        {
            rb.AddForce(Vector3.up * jumpForce);
            jumping = false;
        }
        if(rb.velocity.y > maxJumpSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxJumpSpeed, rb.velocity.z);
        }
        Falling();
    }
    private void Falling()
    {
        if(!jumping && rb.velocity.y < 0)
        {
            rb.mass = gravityMultiplier;
        }
        if(rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxFallSpeed, rb.velocity.z);
        }
    }
    private void GroundCheck()
    {       
        if(Physics.Raycast(transform.position, -transform.up, .1f, layerMask))
        {            
            grounded = true;
            rb.mass = originalMass;
        }
        else
            grounded = false;
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
        /*else 
            Debug.Log("No Item Selected");*/    


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