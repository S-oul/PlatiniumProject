using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    public Animator _animator;
    bool _isMirrored = false;

    [Header ("For Game Design")]   
    [Range(1, 100)][SerializeField] float _moveSpeed = 40f;                 // Movement speed                 
    [Range(0, .3f)] public float movementSmoothing = .05f;                  // How much to smooth out the movement
    [Range(0.1f, 20f)]public float normalFallGravityForce = 3;              // Fall Speed
    [SerializeField] public bool AirControl = true;                         // Can contoll character while not Grounded
    [SerializeField] private float _jumpForce = 400f;

    string _name;
    PlayerManager.ControllerType _type;

    ParticleSystem _runParticles;

    float _baseMoveSpeed;
    float _baseJumpForce;
    float _horizontalMove = 0f;
    bool _isJumping = false;
    bool _isGrounded = false;                                                // why did they replace _isGrounded with _isPlayerDown?
    bool _isInteracting = false;
    bool _isPlayerDown = false;
    bool _isBlocked = false;
    bool _walkingSoundCanPlay = false;
    bool _canMove = true;

    CharacterController2D _controller;
    Collider2D _colliderPlayer;
    Rigidbody2D _rb;
    AudioSource _audioSource;

    public Gamepad pad = Gamepad.current;

    string _codeContext;
    Vector2 _DecrytContext;
    Vector2 _joystickContext;
    Vector2 _graffitiContext;

    public string currentContextName;

    Coroutine _walkCoroutine;

    public Color _colorPlayer;

    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool IsPlayerDown { get => _isPlayerDown; set => _isPlayerDown = value; }
    public string CodeContext { get => _codeContext; set => _codeContext = value; }
    public Vector2 DecrytContext { get => _DecrytContext; set => _DecrytContext = value; }
    public Vector2 JoystickContext { get => _joystickContext; set => _joystickContext = value; }
    public Vector2 GraffitiContext { get => _graffitiContext; set => _graffitiContext = value; }
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    public PlayerManager.ControllerType Type { get => _type; set => _type = value; }
    public Color ColorPlayer { get => _colorPlayer; set => _colorPlayer = value; }
    public string Name { get => _name; set => _name = value; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _rb = GetComponent<Rigidbody2D>();
        _baseJumpForce = _jumpForce;
        _baseMoveSpeed = _moveSpeed;
        _audioSource = gameObject.transform.Find("AudioSource").GetComponent<AudioSource>();
        _walkingSoundCanPlay = true;
        _runParticles = _runParticles = _controller.AllParticles.Find("Run").GetChild(0).GetComponent<ParticleSystem>();
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
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Enable();
        _canMove = false;
    }
    public void DisableInteractMovementEnableInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
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
        _isPlayerDown = true;
        DisableAllInputs();
        _animator.SetTrigger("Down");
        StartCoroutine(dawait());
    }
    IEnumerator dawait()
    {
        yield return new WaitForSeconds(1f);
        transform.localEulerAngles = new Vector3(0, 0, 90);
    }
    private void UpPlayer()
    {
        EnableMovementDisableInputs();
        transform.localPosition += Vector3.up * 0.1f;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        _isPlayerDown = false;
    }

    public void EnableMovementInteractDisableInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _playerInput.actions["Code"].Disable();

        _canMove = true;

    }
    public void EnableMovementDisableInputs()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Disable();
        _playerInput.actions["Code"].Disable();

        _canMove = true;

    }

    public void DisableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _canMove = false;

    }

    public void EnableDecryptageDisableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Decryptage"].Enable();
        _canMove = false;

    }

    public void EnableGraffitiDisableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Graffiti"].Enable();
        _canMove = false;

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
    public void DisableGraffitiEnableMovements()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Interact"].Enable();
        _playerInput.actions["Movement"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["InputTask"].Disable();
        _playerInput.actions["Graffiti"].Disable();
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
            print("interact");
            _isInteracting = true;
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
            _DecrytContext = context.ReadValue<Vector2>();
        }
        else
        {
            _DecrytContext = Vector2.zero;
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
            _joystickContext = Vector2.one;
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("CONTROLLER COFFEE");
            GameManager.Instance.SetPause();
        }
    }

    public void OnGraffiti(InputAction.CallbackContext context)
    { 
        if (context.performed)
        {
            _graffitiContext = context.ReadValue<Vector2>();
        }
        else
        {
            _graffitiContext = Vector2.zero;
        }
            
    }

    // Comunicate contol inputs to CharacterContoller2D Script component
    private void FixedUpdate()
    {
        
        _controller.Movement(_horizontalMove * Time.fixedDeltaTime, _isJumping);
        _isJumping = false;

        if (_horizontalMove != 0) 
        { 
            _animator.SetBool("isWalking", true);
            if (_walkingSoundCanPlay)
            {
                _walkCoroutine = StartCoroutine(PlaySoundWalking());
            }
        }
        else { _animator.SetBool("isWalking", false); }

        if (_horizontalMove < 0 && _isMirrored == false) 
        { 
            flipAnimation(); 
            _isMirrored = true;
            if(_walkCoroutine != null)
            {
                StopCoroutine(_walkCoroutine);
                _runParticles.Stop();
                _walkingSoundCanPlay = true;
            }
            _runParticles = _controller.AllParticles.Find("Run").GetChild(1).GetComponent<ParticleSystem>();  
        }
        else if (_horizontalMove > 0 &&  _isMirrored == true) 
        { 
            flipAnimation(); 
            _isMirrored = false;
            if (_walkCoroutine != null)
            {
                StopCoroutine(_walkCoroutine);
                _runParticles.Stop();
                _walkingSoundCanPlay = true;
            }
            _runParticles = _controller.AllParticles.Find("Run").GetChild(0).GetComponent<ParticleSystem>(); 
        }
    }

    private void flipAnimation()
    {
        Transform animTrans = _animator.GetComponent<Transform>();
        animTrans.localScale = new Vector3(animTrans.localScale.x *-1, animTrans.localScale.y, animTrans.localScale.z);
        //animTrans.rotation = new Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    public IEnumerator PlayerDown(float time)
    {
        DownPlayer();
        yield return new WaitForSeconds(time);
        UpPlayer();   
    }

    IEnumerator PlaySoundWalking()
    {
        _walkingSoundCanPlay = false;
        AudioClip clip = AudioManager.Instance.FindClip("PlayerWalk");
        AudioManager.Instance.PlaySFXOS(clip, _audioSource);
        //print("Audio");

        _runParticles.Play();
        yield return new WaitForSeconds(clip.length + 0.4f);
        _walkingSoundCanPlay = true;
        _walkCoroutine = null;
    }

    private void Update()
    {
        if (_isBlocked)
        {
            _rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.OpenTheFinalDoor();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.SetPause();
        }
    }

    public IEnumerator RumbeMeDaddyOhYesHarderHarder(float lowfreq, float highfreq, float time)
    {
        print("RABLIMG");
        pad.SetMotorSpeeds(lowfreq, highfreq);
        yield return new WaitForSecondsRealtime(time);
        pad.SetMotorSpeeds(0, 0);
    }
    public void ChangeMobiltyFactor(float moveSpeedFactor, float jumpForceFactor)
    {
        _moveSpeed = _baseMoveSpeed * moveSpeedFactor;
        _jumpForce = _baseJumpForce * jumpForceFactor;
    }

    public void DisconnectPlayer()
    {
        foreach(GameObject player in GameManager.Instance.Players)
        {
            if(gameObject == player)
            {
                GameManager.Instance.Players.Remove(player);
                GameManager.Instance.PlayerCount--;
                PlayerManager.Instance.InputManager.EnableJoining();
                //Delete from PlayerManager's Player Input list
            }
        }
        
    }

    

}
