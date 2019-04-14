using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private Animator playerAnimator;

	private bool attacked;
	private Player player;
	private DashManager dash;

	// Use this for initialization
	void Start () {

		Rigidbody2D playerRigidBody = GetComponent<Rigidbody2D> ();
		player = GetComponent<Player> ();
		dash = GetComponent<DashManager> ();

		player.StartPlayer ();
		dash.StartDash (player, playerRigidBody);
	}
	
	// Update is called once per frame
	void Update () {

		player.UpdatePlayer ();
		dash.UpdateDash ();

		player.ApplyColorOnDamaged ();

		if(attacked)
		{
			playerAnimator.ResetTrigger ("Attack");
			attacked = false;
		}

		if (Input.GetKey (KeyCode.Mouse0) || Input.GetKey (KeyCode.E))
		{
			attacked = true;
			playerAnimator.SetTrigger("Attack");
		}
		if (Input.GetKey (KeyCode.A))
		{
			player.MoveLeft ();
		}
		if (Input.GetKey (KeyCode.D))
		{
			player.MoveRight ();
		}
		if (Input.GetKeyDown (KeyCode.Space))
		{
			player.Jump ();
		}
		if (Input.GetKeyDown (KeyCode.LeftShift))
		{
			dash.Dash ();
		}
		
	}
}
