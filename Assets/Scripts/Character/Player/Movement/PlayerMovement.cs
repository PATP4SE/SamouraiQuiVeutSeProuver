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
	[SerializeField] private float jumpSpeed;
	[SerializeField] private float fallingMultiplier;
	[SerializeField] private int numberOfJumps;
    [SerializeField] private float speed;

    [SerializeField] private float invulnerabilityAfterHitDuration;
	[SerializeField] private float secondsBetweenFlashes;

	//[SerializeField] private GameObject dustParticles;

	[SerializeField] private BoxCollider bottomCollider;

	//======================================================
	// Private variables
	//======================================================
	private int jumpNumber;
	private bool isJumping = false;
	private bool isFalling = false;

	private bool damaged;
	private float timeToRemoveFlash;
	private float timeToChangeColor;

    //======================================================
    // Private physics-related variables
    //======================================================
    private Rigidbody playerRigidBody;
    //private SpriteRenderer playerSprite;
    private BoxCollider playerCollider;

    //======================================================
    // Public methodes
    //======================================================
    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        //playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider>();
    }

    public void StartMovement()
	{
        damaged = false;
		isMovementDisabled = false;
	}

	public void UpdateMovement()
	{
		//Utils.DecelerateX(ref characterRigidbody, decelerationPercentage);

		if(playerRigidBody.velocity.y > 0)
		{
			isFalling = true;
			playerRigidBody.velocity += Vector3.up * Physics2D.gravity.y * (fallingMultiplier - 1) * Time.deltaTime;
		}
	}

	public void MoveLeft()
	{
		if (!isMovementDisabled) 
		{
            direction = (int)Directions.Left;
			//playerSprite.flipX = true;
			playerRigidBody.AddForce (new Vector2 (-speed, 0), ForceMode.Impulse);
		}
	}

	public void MoveRight()
	{
		if (!isMovementDisabled) 
		{
			direction = (int)Directions.Right;
			//playerSprite.flipX = false;
			playerRigidBody.AddForce (new Vector2 (speed, 0), ForceMode.Impulse);
		}
	}

	public void Jump()
	{
		if (jumpNumber != 0 && !isMovementDisabled) 
		{
			isJumping = true;
			jumpNumber--;

			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, 0);
			playerRigidBody.AddForce(new Vector2(0, jumpSpeed));
		}
	}	

	//======================================================
	// Event methods
	//======================================================

	//Method called when bottomCollider hits
	void OnTriggerEnter(Collider collider)
	{
		jumpNumber = numberOfJumps;
		isJumping = false;
		isFalling = false;

		//GameObject.Instantiate<GameObject> (dustParticles, transform.position, new Quaternion ());
	}
    
	void OnTriggerExit(Collider collider)
	{
		if (!isJumping /*&& !playerCollider.IsTouchingLayers()*/)
			jumpNumber--;
	}

	void OnCollisionEnter(Collision coll) 
	{
		if (coll.gameObject.tag == "Enemy") 
		{
            Physics.IgnoreCollision(playerCollider, coll.collider);
		}
	}

	//======================================================
	// Private methods
	//======================================================

	

}

