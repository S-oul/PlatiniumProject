using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    private CharacterController2D controller;

    float horisontalMove = 0f;

    public float runSpeed = 40f;
    bool isJumping = false;
    bool isCrouched = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    /*void Update()
    {
        horisontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump")) { isJumping = true; }
        if (Input.GetButtonDown("Crouch")) { isCrouched = true; }
        else if (Input.GetButtonUp("Crouch")) { isCrouched = false; }

    }*/

    public void OnMove(InputAction.CallbackContext context)
    {
        horisontalMove = context.ReadValue<Vector2>().x * runSpeed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        /*isJumping = context.ReadValue<bool>();*/
        isJumping = context.action.triggered;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

    }

    private void FixedUpdate()
    {
        controller.Move(horisontalMove * Time.fixedDeltaTime, isCrouched, isJumping);
        isJumping = false;

    }
}
