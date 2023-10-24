using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    float horisontalMove = 0f;

    public float runSpeed = 40f;
    bool jump = false;


    // Update is called once per frame
    void Update()
    {
        horisontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump")) { jump = true; }
    
    }

    private void FixedUpdate()
    {
        controller.Move(horisontalMove * Time.fixedDeltaTime, jump);
        jump = false;

    }
}
