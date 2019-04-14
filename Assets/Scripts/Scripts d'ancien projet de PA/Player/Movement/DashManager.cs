#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : MonoBehaviour {

	//======================================================
	//Serialized variables
	//======================================================
	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashDistance;
	[SerializeField] private float cooldown;
	[SerializeField] private float lagTimeBeforeDash;
	//[SerializeField] private float decelerationPercentage;

	//======================================================
	//Private physics-related variables
	//======================================================
	private Rigidbody2D playerRig;
	private Player player;

	//======================================================
	//Private variables
	//======================================================
	private bool isDashing = false;
	private float nextTimeToDash;
	private Vector2 dashingVelocity;
	private bool capturedVelocity = false;
	private bool dashPressed = false;

	private float nextTimeToDashBeforeLag;
	private float initialPositionX;

	//======================================================
	//Constants
	//======================================================
	private const int AFTER_DASHING_VELOCITY_X = 9;

	//======================================================
	// Public methods
	//======================================================

	public void StartDash (Player _player, Rigidbody2D _playerRigidBody) {
		playerRig = _playerRigidBody;
		player = _player;
		nextTimeToDash = 0;
	}

	public void Dash()
	{
		if (!isDashing && !dashPressed) 
		{
			LagAndDash();
		}
	}

	public void UpdateDash()
	{
		if (Input.GetKeyDown (KeyCode.LeftShift) && Time.time >= nextTimeToDash) 
		{
			LagAndDash();
		}

		if(Time.time >= nextTimeToDashBeforeLag && dashPressed && !isDashing)
		{
			isDashing = true;
			nextTimeToDash = Time.time + cooldown;
			initialPositionX = transform.position.x;

			dashPressed = false;
		}

		if(isDashing)
		{
			playerRig.AddForce (new Vector2 (player.direction * dashSpeed, 0), ForceMode2D.Impulse);

			if (transform.position.x >= initialPositionX + dashDistance || transform.position.x <= initialPositionX - dashDistance)
				StopDashing ();

			if ((player.direction == 1 && playerRig.velocity.x <= AFTER_DASHING_VELOCITY_X) || (player.direction == -1 && playerRig.velocity.x >= -AFTER_DASHING_VELOCITY_X))
			{
				isDashing = false;

				#if DEBUG
				Debug.Log ("stop dashing");
				Debug.Log (playerRig.velocity.x);
				#endif
			}
		}
	}


	private void StopDashing()
	{
		isDashing = false;
		playerRig.velocity = new Vector2 (9 * player.direction, playerRig.velocity.y);
		player.isMovementDisabled = false;
	}

	private void LagAndDash()
	{
		player.isMovementDisabled = true;
		nextTimeToDashBeforeLag = Time.time + lagTimeBeforeDash;
		dashPressed = true;
	}
}
