using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{

	public static CharacterController2D _controller;
	private Animator animator;

	public float falling_threshold = -15f;
    public bool Falling = false;
    public float flying_threshold = 35f;
    public bool Flying = false;
	public bool on_launchpad = false;

	public bool tracking_airtime = false;
	public float time_in_air = 0f;
	public float top_time = 0f;
	public float launch_multiplyer = 0f;

	// Test launch speed
	public float launcher_thruster = 550f;

	// UI controll
	public Text time_in_air_text;
	public Text top_time_in_air_text;
	public Text multiplier_text;


	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

        if (_controller == null)
        {
            DontDestroyOnLoad(gameObject);
            _controller = this;
        } else if (_controller != this)
        {
            Destroy(gameObject);
        }			
	}

	private void FixedUpdate()
	{
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


	public void Move(float move, bool crouch, bool jump)
	{

		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

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
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			// jump animation
			animator.Play("walt_jump", 0, 0.25f);
			GameCoreLogic._game_logic.PlayAudioJump();
			m_Grounded = false;
		}

		if (move != 0 && m_Grounded)
		{
			animator.SetBool("Run", true);
		} else{
			animator.SetBool("Run", false);
		}

		if (Falling)
		{
			animator.Play("walt_falling", 0, 0.25f);
		} 

		if (Flying)
		{
			animator.Play("walt_flying", 0, 0.25f);
		}		

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("walt_jump"))
		{
			StartCoroutine(delayedGroundCheck());
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("walt_falling"))
		{
			StartCoroutine(delayedGroundCheck());
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("walt_flying"))
		{
			StartCoroutine(delayedGroundCheck());
		}


	

	}


    IEnumerator delayedGroundCheck()
    {
        yield return new WaitForSeconds(0.02f);

		if (m_Grounded)
		{
			animator.Play("walt_idle", 0, 0f);
			time_in_air = 0f;
			tracking_airtime = false;
		}		

    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	void Update () 
	{
		launch_multiplyer = top_time * 100;
		multiplier_text.text = launch_multiplyer.ToString("0");

        if (m_Rigidbody2D.velocity.y < falling_threshold)
        {
            Falling = true;
        }
        else
        {
            Falling = false;
        }

        if (m_Rigidbody2D.velocity.y > flying_threshold)
        {
            Flying = true;
        }
        else
        {
            Flying = false;
        }

		if (tracking_airtime) 
		{
			FollowCamera._camera_controll.ZoomOut();
		} else {
			FollowCamera._camera_controll.ZoomRegular();
		}
		
		// Tracking Time
		if (tracking_airtime)
		{
			on_launchpad = false;
			time_in_air += Time.deltaTime;
			time_in_air_text.text = time_in_air.ToString("0");
			
			if (time_in_air > top_time)
			{
				top_time = time_in_air;
				top_time_in_air_text.text = top_time.ToString("0");
			}
		}		


	}

	public void LaunchPlayer()
	{
		if (on_launchpad)
		{	
			m_Rigidbody2D.AddForce(new Vector2(0f, launcher_thruster + launch_multiplyer));
			tracking_airtime = true;
			time_in_air = 0f;
		}

	}



	//  EOL

}