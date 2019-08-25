#define DEBUG

using UnityEngine;

public class Enemy : Character
{
	[SerializeField] private float _rangeToAttack;
	[SerializeField] private GameObject _player;
	[SerializeField] private float _secondsToFlash;

	private bool _damaged;
	private float _timeToRemoveFlash;
	//private float timeToChangeColor;

	//private const float SECONDS_BETWEEN_FLASHES = 0.1f;

	//======================================================
	// Public methods
	//======================================================

	void Start()
	{
		_damaged = false;
	}

	public void Attack()
	{
		int dir = 0;

		if (GetComponent<SpriteRenderer> ().flipX)
			dir = -1;	
		else
			dir = 1;

		Vector2 highPos = new Vector2 (transform.position.x, transform.position.y + 0.5f);
		Vector2 midPos = transform.position;
		Vector2 lowPos = new Vector2 (transform.position.x, transform.position.y - 0.5f);

		Vector2 direction = new Vector2 (dir * _attackRange, 0);


		RaycastHit2D[] highRay = Physics2D.RaycastAll (highPos, new Vector2 (dir, 0), _attackRange);
		RaycastHit2D[] midRay = Physics2D.RaycastAll (midPos, new Vector2 (dir, 0), _attackRange);
		RaycastHit2D[] lowRay = Physics2D.RaycastAll (lowPos, new Vector2 (dir, 0), _attackRange);

		#if DEBUG
		Debug.DrawRay (highPos, direction);
		Debug.DrawRay (midPos, direction);
		Debug.DrawRay (lowPos, direction);
		#endif

		Collider2D colliderHit = null;

		foreach(RaycastHit2D ray in highRay)
		{
			if(ray.collider != null && ray.collider.gameObject.tag == "Player")
			{
				colliderHit = ray.collider;
				#if DEBUG
				Debug.Log(ray.collider.gameObject.tag);
				#endif
				PushPlayerOnAttack (ray.collider);
			}
		}

		foreach(RaycastHit2D ray in midRay)
		{
			if(ray.collider != null && ray.collider.gameObject.tag == "Player")
			{
				colliderHit = ray.collider;
				#if DEBUG
				Debug.Log(ray.collider.gameObject.tag);
				#endif
				PushPlayerOnAttack (ray.collider);
			}
		}

		foreach(RaycastHit2D ray in lowRay)
		{
			if(ray.collider != null && ray.collider.gameObject.tag == "Player")
			{
				colliderHit = ray.collider;
				#if DEBUG
				Debug.Log(ray.collider.gameObject.tag);
				#endif

				PushPlayerOnAttack (ray.collider);
			}
		}

		if(colliderHit != null)
		{
			colliderHit.gameObject.GetComponent<Player> ().LoseHealth(_attackDamage);
		}
	}

	public float GetRangeToAttack()
	{
		return _rangeToAttack;
	}

	public override void LoseHealth(float damage)
	{
		base.LoseHealth(damage);

		characterSprite.color = Color.red;
		_damaged = true;
		_timeToRemoveFlash = Time.time + _secondsToFlash;
		//timeToChangeColor = Time.time + SECONDS_BETWEEN_FLASHES;
	}

	public void ApplyColorOnDamaged()
	{
		if(_damaged)
		{
//			if (Time.time >= timeToChangeColor) 
//			{
//				timeToChangeColor = Time.time + SECONDS_BETWEEN_FLASHES;
//				characterSprite.color = (characterSprite.color == Color.white) ? Color.red : Color.white;
//			}

			if(Time.time >= _timeToRemoveFlash)
			{
				_damaged = false;
				characterSprite.color = Color.white;
			}
		}
	}

	public override void Die()
	{
		GameObject.Destroy (gameObject);
	}

	#region Private Methods
	private void PushPlayerOnAttack(Collider2D playerCollider)
	{

		if (!playerCollider.gameObject.GetComponent<Player> ().IsInvulnerable ()) 
		{
			Vector2 direction = playerCollider.GetComponent<Rigidbody2D> ().transform.position - this.transform.position;
			playerCollider.GetComponent<Rigidbody2D> ().AddForceAtPosition (direction.normalized * _attackPushForce, this.transform.position);
		}
	}
	#endregion

//	public abstract void SpecialAttack();
//	public override void LoseHealth(float damage);
//	public override void Attack();
//	public override void Dies();
//	public override void Spawns();
}
