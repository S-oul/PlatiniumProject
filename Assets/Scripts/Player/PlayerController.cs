using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [Header ("For Game Design")]   
    [Range(1, 100)][SerializeField] float _moveSpeed = 40f;                 //Movement speed                 
    [Range(0, .3f)] public float movementSmoothing = .05f;         // How much to smooth out the movement
    [Range(0.1f, 20f)]public float normalFallGravityForce = 3;  // Fall Speed
    [SerializeField] public bool AirControl = true;                      // Can contoll character while not Grounded


    float _horizontalMove = 0f;
    bool _isJumping = false;
    bool _isGrounded = false;
    bool _isInteracting = false;
    bool _isPlayerDown = false;

    bool _canMove = true;

    CharacterController2D _controller;
    Collider2D _colliderPlayer;

    public string currentContextName;

    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool IsPlayerDown { get => _isPlayerDown; set => _isPlayerDown = value; }

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
    
    public void DisableMovementExceptInput()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Enable();
        _canMove = false;
    }

    public void DisableMovement()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _canMove = false;
    }
    public void DownPlayer()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _canMove = false;
        _isPlayerDown = true;
    }
    public void EnableMovement()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _canMove = true;
        _isPlayerDown = false;

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
        
        if (context.performed)
        {
            _isInteracting = true ;
            
        }
        else
        {
            _isInteracting = false;
        }
    }
    // Comunicate contol inputs to CharacterContoller2D Script component
    private void FixedUpdate()
    {
        if(_isPlayerDown) { transform.localEulerAngles = new Vector3(0,0,90); }
        else { transform.localEulerAngles = new Vector3(0, 0, 0); }
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _isJumping);
        _isJumping = false;
    }

    
}
