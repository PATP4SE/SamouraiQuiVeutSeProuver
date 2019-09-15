using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement2D : MonoBehaviour
{

    //======================================================
    // Serialized variables
    //======================================================
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[SerializeField] private const int MAX_JUMP_NUMBER = 2;                     // Number of jumps allowed before touching the ground
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    [SerializeField] private float m_Speed = 40f;                               // Speed
    [SerializeField] private float m_fallingMultiplier;                         // How much to smooth out the falling after jumping
    [SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching


    //======================================================
    // Private variables
    //======================================================
    private Rigidbody _rigidbody;
    private Vector3 _velocity = Vector3.zero;

    private const float GROUNDED_RADIUS = .2f; // Radius of the overlap circle to determine if grounded
    private const float CEILLING_RADIUS = .2f; // Radius of the overlap circle to determine if the player can stand up

    private bool _isGrounded;            // Whether or not the player is grounded.
	private bool _isFacingRight = true;  // For determining which way the player is currently facing.
    private int _jumpNumber;
    

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
        _jumpNumber = MAX_JUMP_NUMBER;

        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = _isGrounded;
		_isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, GROUNDED_RADIUS, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
                HitGround(wasGrounded);
			}
		}

        IncreaseFallingSpeed();
    }

	public void Move(float move, bool crouch, bool jump)
	{
        move *= m_Speed;
        // If crouching, check to see if the character can stand up
        if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics.OverlapSphere(m_CeilingCheck.position, CEILLING_RADIUS, m_WhatIsGround).Length > 0)
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (_isGrounded || m_AirControl)
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
			}
            else
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
			Vector3 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
			// And then smoothing it out and applying it to the character
			_rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !_isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && _isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
        // If the player should jump...
        Jump(jump);

    }

    private void Jump(bool isJumping)
    {
        if (isJumping && _jumpNumber != 0)
        {
            // Add a vertical force to the player.
            _isGrounded = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(new Vector2(0, m_JumpForce), ForceMode.Impulse);
            _jumpNumber--;
        }
    }
    
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_isFacingRight = !_isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    private void HitGround(bool wasGrounded)
    {
        _isGrounded = true;
        if (!wasGrounded)
        {
            Debug.Log("OnLand");
            OnLandEvent.Invoke();
            _jumpNumber = MAX_JUMP_NUMBER;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HitGround(false);

        //GameObject.Instantiate<GameObject>(dustParticles, transform.position, new Quaternion());
    }

    //GanonGame
    //void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (!isJumping && !characterCollider.IsTouchingLayers())
    //        jumpNumber--;
    //}

    //To make the jump more fluid
    private void IncreaseFallingSpeed()
    {        
        if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity += Vector3.up * Physics2D.gravity.y * (m_fallingMultiplier - 1) * Time.deltaTime;
        }
    }
}
