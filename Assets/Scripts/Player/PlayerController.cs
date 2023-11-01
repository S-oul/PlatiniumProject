using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    bool _isMirrored = false;

    [Header ("For Game Design")]   
    [Range(1, 100)][SerializeField] float _moveSpeed = 40f;                 // Movement speed                 
    [Range(0, .3f)] public float movementSmoothing = .05f;                  // How much to smooth out the movement
    [Range(0.1f, 20f)]public float normalFallGravityForce = 3;              // Fall Speed
    [SerializeField] public bool AirControl = true;                         // Can contoll character while not Grounded


    float _horizontalMove = 0f;
    bool _isJumping = false;
    bool _isGrounded = false;                                                // why did they replace _isGrounded with _isPlayerDown?
    bool _isInteracting = false;
    bool _isPlayerDown = false;
    bool _isBlocked = false;

    bool _canMove = true;

    CharacterController2D _controller;
    Collider2D _colliderPlayer;
    Rigidbody2D _rb;

    string _codeContext;
    float _DecrytContext;
    Vector2 _joystickContext;

    public string currentContextName;


    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool IsPlayerDown { get => _isPlayerDown; set => _isPlayerDown = value; }
    public string CodeContext { get => _codeContext; set => _codeContext = value; }
    public float DecrytContext { get => _DecrytContext; set => _DecrytContext = value; }
    public Vector2 JoystickContext { get => _joystickContext; set => _joystickContext = value; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _rb = GetComponent<Rigidbody2D>();
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
    public void BlockPlayer(bool value)
    {
        if (value)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
       
        _isBlocked = value;
        
    }
    public void DisableMovementEnableInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Enable();
        _canMove = false;
    }
    public void DisableAllInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Joystick left"].Disable();

        _canMove = false;
    }
    public void JoystickOnlyInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Joystick left"].Enable();
        _canMove = false;
    }
    private void DownPlayer()
    {
        DisableAllInputs();
        _isPlayerDown = true;
    }
    private void UpPlayer()
    {
        EnableMovementDisableInputs();
        _isPlayerDown = false;
    }
    public void EnableMovementDisableInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _canMove = true;

    }

    public void DisableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _canMove = true;

    }

    public void EnableDecryptageDisableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Enable();
        _canMove = true;

    }

    public void DisableDecryptageEnableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _canMove = true;

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
    public void OnCode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _codeContext = context.action.activeControl.displayName;
        }
        else
        {
            _codeContext = "";
        }
    }
    public void OnDecryptage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _DecrytContext = context.ReadValue<Vector2>().x;
        }
        else
        {
            _DecrytContext = 0;
        }
    }

    public void OnJoystickLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _joystickContext = context.ReadValue<Vector2>();
        }
        else
        {
            _joystickContext = Vector2.zero;
        }
    }
    // Comunicate contol inputs to CharacterContoller2D Script component
    private void FixedUpdate()
    {
        if (_isPlayerDown) { transform.localEulerAngles = new Vector3(0, 0, 90); }
        else { transform.localEulerAngles = new Vector3(0, 0, 0); }
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _isJumping);
        _isJumping = false;

        if (_horizontalMove != 0) { animator.SetBool("isWalking", true); }
        else { animator.SetBool("isWalking", false); }

        if (_horizontalMove < 0 && _isMirrored == false) { flipAnimation(); _isMirrored = true; }
        else if (_horizontalMove > 0 &&  _isMirrored == true) { flipAnimation(); _isMirrored = false; }
    }

    private void flipAnimation()
    {
        Transform animTrans = animator.GetComponent<Transform>();
        animTrans.localScale = new Vector3(animTrans.localScale.x *-1, animTrans.localScale.y, animTrans.localScale.z);
        //animTrans.rotation = new Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    public IEnumerator PlayerDown(float time)
    {
        DownPlayer();
        yield return new WaitForSeconds(time);
        UpPlayer();   
    }

    private void Update()
    {
        if (_isBlocked)
        {
            _rb.velocity = Vector2.zero;
        }
    }
}
