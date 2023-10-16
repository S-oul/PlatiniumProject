using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [Header ("For Game Design")]   
    [Range(1, 100)][SerializeField] float _moveSpeed = 40f;                 //Movement speed
    [Range(0, 1)] public float crouchSpeed = .36f;                 // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] public float movementSmoothing = .05f;         // How much to smooth out the movement
    [Range(0.1f, 20f)]public float normalFallGravityForce = 3;  // Fall Speed
    [Range(0.1f, 20f)]public float fastFallGravityForce = 5;    // Fall Speed when holding Crouch
    [SerializeField] public bool AirControl = true;                      // Can contoll character while not Grounded


    float _horizontalMove = 0f;
    bool _isJumping = false;
    bool _isGrounded = false;
    bool _isCrouched = false;
    public bool _isInteracting = false;

    CharacterController2D _controller;
    Collider2D _colliderPlayer;

    public string currentContextName;

    private void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        
    }


    // Functions called by the Player Input component. 
    public void OnMove(InputAction.CallbackContext context)
    {
        _horizontalMove = context.ReadValue<Vector2>().x * _moveSpeed;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        _isJumping = context.ReadValueAsButton();
        _isJumping = context.action.triggered;
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        _isCrouched = context.ReadValueAsButton();
    }
    

    public void OnInputTask(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            currentContextName = context.action.activeControl.displayName;
        }
        else
        {
            currentContextName = "";
        }
        
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        _isInteracting = context.ReadValueAsButton();
    }
    // Comunicate contol inputs to CharacterContoller2D Script component
    private void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _isCrouched, _isJumping);
        _isJumping = false;
    }

    
}
