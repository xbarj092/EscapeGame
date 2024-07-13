using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;

	const float k_GroundedRadius = .55f; // Radius of the overlap circle to determine if grounded
	public static bool m_Grounded { get; private set; }
    private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("TOGGLE ABILITIES")]
	public bool canDash = false;
    public bool canCoyoteJump = false;
    public bool canDoubleJump = false;



    private float _time;
	private bool wantsToJump;

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private float _leftGroundAt;
    private float coyoteJumpOffset = 0.1f;
	private bool doubleJumpCharge;

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				doubleJumpCharge = true;
			}
		}
		if (wasGrounded && !m_Grounded)
		{
			_leftGroundAt = _time;
		}
	}

	public void JumpPressed()
	{
		wantsToJump = true;
	}

    private void Update()
    {
        _time += Time.deltaTime;
    }

    private bool coyoteJumpPossible => canCoyoteJump && !m_Grounded && _time < _leftGroundAt + coyoteJumpOffset;
    private bool doubleJumpPossible => canDoubleJump && !m_Grounded && doubleJumpCharge;

    public void Move(float move)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight)) 
		{
			Flip();
		}

		if (wantsToJump)
		{
            if (m_Grounded)
            {
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
			else
			{
				if (coyoteJumpPossible)
				{
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				}
                if (doubleJumpPossible)
                {
                    //m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                    // Reset the vertical velocity
                    Vector2 currentVelocity = m_Rigidbody2D.velocity;
                    currentVelocity.y = 0;
                    m_Rigidbody2D.velocity = currentVelocity;

                    // Apply the jump force
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                    doubleJumpCharge = false;
                }
            }
        }
		wantsToJump = false;
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
}
