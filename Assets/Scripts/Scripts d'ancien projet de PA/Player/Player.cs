#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character 
{

    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDistance;
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

	[SerializeField] private float invulnerabilityAfterHitDuration;
	[SerializeField] private float secondsBetweenFlashes;

	[SerializeField] private GameObject dustParticles;

	[SerializeField] private BoxCollider2D bottomCollider;

	//======================================================
	// Private variables
	//======================================================
	private int jumpNumber;
	private bool isJumping = false;
	private bool isFalling = false;

	private bool damaged;
	private float timeToRemoveFlash;
	private float timeToChangeColor;

	void Start () {
	}

	public void StartPlayer()
	{
		damaged = false;
		isMovementDisabled = false;
	}

	//======================================================
	// Public methodes
	//======================================================
	public void UpdatePlayer()
	{
		Utils.DecelerateX(ref characterRigidbody, decelerationPercentage);

		if(characterRigidbody.velocity.y > 0)
		{
			isFalling = true;
			characterRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallingMultiplier - 1) * Time.deltaTime;
		}
	}

	public void MoveLeft()
	{
		if (!isMovementDisabled) 
		{
			direction = (int)Direction.Left;
			characterSprite.flipX = true;
			characterRigidbody.AddForce (new Vector2 (-speed, 0), ForceMode2D.Impulse);
		}
	}

	public void MoveRight()
	{
		if (!isMovementDisabled) 
		{
			direction = (int)Direction.Right;
			characterSprite.flipX = false;
			characterRigidbody.AddForce (new Vector2 (speed, 0), ForceMode2D.Impulse);
		}
	}

	public void Jump()
	{
		if (jumpNumber != 0 && !isMovementDisabled) 
		{
			isJumping = true;
			jumpNumber--;

			characterRigidbody.velocity = new Vector2 (characterRigidbody.velocity.x, 0);
			characterRigidbody.AddForce(new Vector2(0, jumpSpeed));
		}
	}

	public void Attack()
	{
		int direction = (int)Direction.None;

		if (GetComponent<SpriteRenderer> ().flipX)
			direction = (int)Direction.Left;	
		else
			direction = (int)Direction.Right;

		Vector2 highPosition = new Vector2 (transform.position.x, transform.position.y + 0.5f);
		Vector2 midPosition = transform.position;
		Vector2 lowPosition = new Vector2 (transform.position.x, transform.position.y - 0.5f);

		Vector2 directionVector = new Vector2 (direction * attackRange, 0);

		RaycastHit2D[] highRay = Physics2D.RaycastAll (highPosition, new Vector2 (direction, 0), attackRange);
		RaycastHit2D[] midRay = Physics2D.RaycastAll (midPosition, new Vector2 (direction, 0), attackRange);
		RaycastHit2D[] lowRay = Physics2D.RaycastAll (lowPosition, new Vector2 (direction, 0), attackRange);

		#if DEBUG
		Debug.DrawRay (highPosition, directionVector);
		Debug.DrawRay (midPosition, directionVector);
		Debug.DrawRay (lowPosition, directionVector);
		#endif

		List<Enemy> enemyList = new List<Enemy> ();

		//Raycasts
		foreach(RaycastHit2D ray in highRay)
		{
			AttackEnemyOnRayCastHit (ray, enemyList);
		}

		foreach(RaycastHit2D ray in midRay)
		{
			AttackEnemyOnRayCastHit (ray, enemyList);
		}

		foreach(RaycastHit2D ray in lowRay)
		{
			AttackEnemyOnRayCastHit (ray, enemyList);
		}

		//Enemy lose health
		foreach (Enemy enemy in enemyList) 
		{
			enemy.LoseHealth (attackDamage);
		}
	}

	public override void LoseHealth(float damage)
	{		
		if (!damaged) 
		{
			base.LoseHealth (damage);
		
			characterSprite.color = Color.red;
			damaged = true;
			timeToRemoveFlash = Time.time + invulnerabilityAfterHitDuration;
			timeToChangeColor = Time.time + secondsBetweenFlashes;
		}
	}

	public void ApplyColorOnDamaged()
	{
		if(damaged)
		{
			if (Time.time >= timeToChangeColor) 
			{
				timeToChangeColor = Time.time + secondsBetweenFlashes;
				characterSprite.color = (characterSprite.color == Color.white) ? Color.red : Color.white;
			}

			if(Time.time >= timeToRemoveFlash)
			{
				damaged = false;
				characterSprite.color = Color.white;
			}
		}
	}

	public override void Die()
	{
		GameObject.Destroy (gameObject);
	}

	public bool IsInvulnerable()
	{
		return damaged;
	}

	//======================================================
	// Event methods
	//======================================================

	//Method called when bottomCollider hits
	void OnTriggerEnter2D(Collider2D collider)
	{
		jumpNumber = numberOfJumps;
		isJumping = false;
		isFalling = false;

		GameObject.Instantiate<GameObject> (dustParticles, transform.position, new Quaternion ());
	}


	void OnTriggerExit2D(Collider2D collider)
	{
		if (!isJumping && !characterCollider.IsTouchingLayers())
			jumpNumber--;
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Enemy") 
		{
			Physics2D.IgnoreCollision (characterCollider, coll.collider);
		}
	}

	//======================================================
	// Private methods
	//======================================================

	private void AttackEnemyOnRayCastHit(RaycastHit2D raycast, List<Enemy> enemyList )
	{
		#if DEBUG
		Debug.Log(raycast.collider.gameObject.tag);
		#endif

		Enemy enemy = raycast.collider.gameObject.GetComponent<Enemy> ();

		PushEnemyOnAttack (raycast.collider);

		if (enemy != null && !enemyList.Contains (enemy))
			enemyList.Add (enemy);
	}

	private void PushEnemyOnAttack(Collider2D enemyCollider)
	{
		Vector2 direction = enemyCollider.GetComponent<Rigidbody2D>().transform.position - this.transform.position;
		enemyCollider.GetComponent<Rigidbody2D>().AddForceAtPosition(direction.normalized * attackPushForce, this.transform.position);
	}

}

