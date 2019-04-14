using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
	//======================================================
	// Public variables
	//======================================================
	public float Health;
	public float Defense;

	//======================================================
	// Serialized variables
	//======================================================
	[SerializeField] protected float speed;
	[SerializeField] protected float decelerationPercentage;

	[SerializeField] protected float attackRange;
	[SerializeField] protected float attackPushForce;
	[SerializeField] protected float attackDamage;

	//======================================================
	// Private physics-related variables
	//======================================================
	protected Rigidbody2D characterRigidbody;
	protected SpriteRenderer characterSprite;
	protected BoxCollider2D characterCollider;

	void Awake () {
		characterRigidbody = GetComponent<Rigidbody2D> ();
		characterSprite = GetComponent<SpriteRenderer> ();
		characterCollider = GetComponent<BoxCollider2D> ();
	}

	public virtual void LoseHealth(float damage)
	{
		this.Health -= (damage - (this.Defense / 2));
		//Start losing health animation
		if (this.Health <= 0)
			Die ();
	}
	//public abstract void Attack();
	public abstract void Die();
	//public abstract void Spawns();
}

