/*
 * Player Controller
 * 
 * This script is responsible for controlling the player character in a Unity game.
 * It handles character movement, rotation, animation, and input handling.
 * 
 * Attach this script to the player character GameObject.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    // References to other components
    PlayerControls playerControls;               // Player input controls
    CharacterController characterController;     // Character controller component
    Animator animator;                           // Animator component

    // Movement variables
    Vector2 currentMovementInput;                // Current input for movement
    Vector3 currentMovement;                     // Current movement direction
    Vector3 currentRunMovement;                  // Current movement direction while running
    Vector3 currentMovementRaw;                  // Raw movement vector
    Vector3 currentRunMovementRaw;               // Raw movement vector while running
    Vector3 movement;                            // Final movement vector
    Vector2 aimedMovement, currentAimedMovement; // Input for aiming movement
    bool isMovementPressed;                      // Flag indicating if movement input is being pressed
    bool isRunPressed;                           // Flag indicating if run input is being pressed
    bool isAiming;                               // Flag indicating if the player is aiming
    Transform cameraMainTransform;               // Reference to the main camera's transform

    // Configuration variables
    public float walkSpeed = 1f;                 // Walking speed
    public float runSpeed = 3f;                  // Running speed
    public float rotationSpeed = 1f;             // Rotation speed
    public float acceleration = 1f;              // Movement acceleration
    public float deceleration = 1f;              // Movement deceleration
    public float aimingWalkSpeed = 1f;           // Walking speed while aiming

    void Awake()
    {
        // Get references to components and initialize variables
        playerControls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraMainTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Set up event handlers for movement and run inputs
        playerControls.Movement.Walk.started += OnMovementInput;
        playerControls.Movement.Walk.canceled += OnMovementInput;
        playerControls.Movement.Walk.performed += OnMovementInput;
        playerControls.Movement.Run.started += OnRunPressed;
        playerControls.Movement.Run.canceled += OnRunPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        aimedMovement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Update movement vectors based on camera orientation
        GetAnimatorValues();
        currentMovement = cameraMainTransform.forward * currentMovementRaw.z + cameraMainTransform.right * currentMovementRaw.x;
        currentMovement.y = 0f;
        currentRunMovement = cameraMainTransform.forward * currentRunMovementRaw.z + cameraMainTransform.right * currentRunMovementRaw.x;
        currentRunMovement.y = 0f;

        // Apply movement
        if (isRunPressed)
        {
            movement = Vector3.Lerp(movement, currentRunMovement, Time.deltaTime * (currentRunMovement.magnitude > movement.magnitude ? acceleration : deceleration));
            characterController.Move(movement * Time.deltaTime);
        }
        else
        {
            movement = Vector3.Lerp(movement, currentMovement, Time.deltaTime * (currentMovement.magnitude > movement.magnitude ? acceleration : deceleration));
            characterController.Move(movement * Time.deltaTime);
        }

        // Handle rotation and animation
        HandleRotation();
        if (isAiming)
        {
            HandleAimedRotation();
            aimedMovement = Vector2.Lerp(aimedMovement, currentAimedMovement, 5f);
            animator.SetFloat("AimedWalkForward", aimedMovement.y);
            animator.SetFloat("AimedWalkLeft", aimedMovement.x);
        }
        else
        {
            HandleRotation();
        }
    }

    // Get the current values from the animator
    private void GetAnimatorValues()
    {
        isAiming = animator.GetBool("Aiming");
    }

    // Event handler for the run input
    void OnRunPressed(InputAction.CallbackContext ctx)
    {
        isRunPressed = ctx.ReadValueAsButton();
    }

    // Handle rotation of the character based on movement input
    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt = new Vector3(currentMovement.x, 0f, currentMovement.z); 

        Quaternion currentRotation = transform.rotation;
        
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Handle rotation of the character while aiming
    void HandleAimedRotation()
    {
        Quaternion currentRotation = transform.rotation;
        currentMovement = cameraMainTransform.forward;
        Vector3 positionToLookAt = new Vector3(currentMovement.x, 0f, currentMovement.z);
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Handle animation based on movement and aiming inputs
    void HandleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (!isAiming)
        {
            if (isMovementPressed && !isWalking)
            {
                animator.SetBool("isWalking", true);
            }
            else if (!isMovementPressed && isWalking)
            {
                animator.SetBool("isWalking", false);
            }

            if (isRunPressed && !isRunning && isWalking)
            {
                animator.SetBool("isRunning", true);
            }
            else if (!isRunPressed && isRunning || !isWalking)
            {
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            if (isMovementPressed && !isWalking)
            {
                animator.SetBool("isWalking", true);
            }
            else if (!isMovementPressed && isWalking)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    // Event handler for movement input
    void OnMovementInput(InputAction.CallbackContext ctx)
    {
        GetComponent<InteractController>().interacted = false;
        currentMovementInput = ctx.ReadValue<Vector2>();

        currentAimedMovement = currentMovementInput;

        if (!isAiming)
        {
            currentMovementRaw = new Vector3(currentMovementInput.x * walkSpeed, 0, currentMovementInput.y * walkSpeed);
            currentRunMovementRaw = new Vector3(currentMovementInput.x * runSpeed, 0, currentMovementInput.y * runSpeed);
        }
        else
        {
            currentMovementRaw = new Vector3(currentMovementInput.x * aimingWalkSpeed, 0, currentMovementInput.y * aimingWalkSpeed);
            currentRunMovementRaw = new Vector3(currentMovementInput.x * aimingWalkSpeed, 0, currentMovementInput.y * aimingWalkSpeed);
        }
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    // Convert rotation in degrees to a normalized vector
    Vector2 RotationToVector(float degrees)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, degrees);
        Vector2 v = rotation * Vector3.down;

        return v;
    }

    void OnEnable()
    {
        // Enable player input
        playerControls.Movement.Enable();
    }

    void OnDisable()
    {
        // Disable player input
        playerControls.Movement.Disable();
    }
}