using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterController2D : MonoBehaviour
{
	private float m_JumpForce;							// Amount of force added when the player jumps.
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	//[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	//[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded. 
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
																				//[SerializeField] private float m_normalFallGravityForce = 3;
																				//[SerializeField] private float m_fastFallGravityForce = 5;
	// PlayerController and values to retreive from PlayerControler
	PlayerController _playerController;
	float m_MovementSmoothing;
	bool m_AirControl;

    float m_normalFallGravityForce;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

	private Vector3 m_Velocity = Vector3.zero;
	private SpriteRenderer m_SpriteRenderer;

	AudioSource m_AudioSource;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;

    // Information for PlayerStateManager
    [HideInInspector] public bool isIdle { get; private set; }


    private void Awake()
	{
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		_playerController = GetComponent<PlayerController>();
        m_AudioSource = gameObject.transform.Find("AudioSource").GetComponent<AudioSource>();

        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}

	private void FixedUpdate()
	{
        // retreive variables from PlayerController 
        m_JumpForce = _playerController.JumpForce;
        m_MovementSmoothing = _playerController.movementSmoothing;
		m_AirControl = _playerController.AirControl;
		m_normalFallGravityForce = _playerController.normalFallGravityForce;

        bool wasGrounded = m_Grounded;
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

        // If the player should jump...
        if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

			// Trigger Player Jump animation. 
			GetComponentInChildren<Animator>().SetTrigger("JumpAnimationTrigger");

			AudioManager.instance.PlaySFXOS("PlayerJump", m_AudioSource);
        }
		
    }


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;



        // Multiply the player's x local scale by -1.



        /*Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;*/



        m_SpriteRenderer.flipX = !m_FacingRight;





    }


}
