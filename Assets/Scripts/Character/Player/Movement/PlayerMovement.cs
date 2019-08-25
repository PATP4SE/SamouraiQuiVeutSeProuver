#define DEBUG

using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    //======================================================
	// Public variables
	//======================================================
	public int direction {get; set;}
	public bool isMovementDisabled {get; set;}

	//======================================================
	// Serialized variables
	//======================================================
	[SerializeField] private float _jumpSpeed;
	[SerializeField] private float _fallingMultiplier;
	[SerializeField] private int _numberOfJumps;
    [SerializeField] private float _speed;

    [SerializeField] private float _invulnerabilityAfterHitDuration;
	[SerializeField] private float _secondsBetweenFlashes;

	[SerializeField] private BoxCollider _bottomCollider;

	//======================================================
	// Private variables
	//======================================================
	private int _jumpNumber;
	private bool _isJumping = false;
	private bool _isFalling = false;

	private bool _damaged;
	private float _timeToRemoveFlash;
	private float _timeToChangeColor;

    //======================================================
    // Private physics-related variables
    //======================================================
    private Rigidbody _playerRigidBody;
    //private SpriteRenderer playerSprite;
    private BoxCollider _playerCollider;

    //======================================================
    // Public methodes
    //======================================================
    public void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        //playerSprite = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<BoxCollider>();
    }

    public void StartMovement()
	{
        _damaged = false;
		isMovementDisabled = false;
	}

	public void UpdateMovement()
	{
		//Utils.DecelerateX(ref characterRigidbody, decelerationPercentage);

		if(_playerRigidBody.velocity.y > 0)
		{
			_isFalling = true;
			_playerRigidBody.velocity += Vector3.up * Physics2D.gravity.y * (_fallingMultiplier - 1) * Time.deltaTime;
		}
	}

	public void MoveLeft()
	{
		if (!isMovementDisabled) 
		{
            direction = (int)Directions.Left;
			//playerSprite.flipX = true;
			_playerRigidBody.AddForce (new Vector2 (-_speed, 0), ForceMode.Impulse);
		}
	}

	public void MoveRight()
	{
		if (!isMovementDisabled) 
		{
			direction = (int)Directions.Right;
			//playerSprite.flipX = false;
			_playerRigidBody.AddForce (new Vector2 (_speed, 0), ForceMode.Impulse);
		}
	}

	public void Jump()
	{
		if (_jumpNumber != 0 && !isMovementDisabled) 
		{
			_isJumping = true;
			_jumpNumber--;

			_playerRigidBody.velocity = new Vector2 (_playerRigidBody.velocity.x, 0);
			_playerRigidBody.AddForce(new Vector2(0, _jumpSpeed));
		}
	}	

	//======================================================
	// Event methods
	//======================================================

	//Method called when bottomCollider hits
	void OnTriggerEnter(Collider collider)
	{
		_jumpNumber = _numberOfJumps;
		_isJumping = false;
		_isFalling = false;

		//GameObject.Instantiate<GameObject> (dustParticles, transform.position, new Quaternion ());
	}
    
	void OnTriggerExit(Collider collider)
	{
		if (!_isJumping /*&& !playerCollider.IsTouchingLayers()*/)
			_jumpNumber--;
	}

	void OnCollisionEnter(Collision coll) 
	{
		if (coll.gameObject.tag == "Enemy") 
		{
            Physics.IgnoreCollision(_playerCollider, coll.collider);
		}
	}

	//======================================================
	// Private methods
	//======================================================

	

}

