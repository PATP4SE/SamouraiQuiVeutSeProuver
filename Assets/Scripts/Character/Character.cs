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
	//[SerializeField] protected float speed;
	//[SerializeField] protected float decelerationPercentage;

	[SerializeField] protected float _attackRange;
	[SerializeField] protected float _attackPushForce;
	[SerializeField] protected float _attackDamage;

	//======================================================
	// Private physics-related variables
	//======================================================
	protected SpriteRenderer characterSprite;

	void Awake () {
		characterSprite = GetComponent<SpriteRenderer> ();
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

