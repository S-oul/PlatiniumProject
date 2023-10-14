using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 40f;


    

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
    private void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _isCrouched, _isJumping);
        _isJumping = false;
    }

    
}
