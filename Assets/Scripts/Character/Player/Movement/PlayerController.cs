using UnityEngine;

public class PlayerController : MonoBehaviour {

	//[SerializeField] private Animator playerAnimator;

	private bool attacked;
	private PlayerMovement playerMovement;
	private Player player;

    // Use this for initialization
    void Start ()
    {
        Rigidbody2D playerRigidBody = GetComponent<Rigidbody2D> ();
		playerMovement = GetComponent<PlayerMovement> ();
        player = GetComponent<Player> ();

        playerMovement.StartMovement ();
	}

	// Update is called once per frame
	void Update ()
    {
		playerMovement.UpdateMovement ();

        player.ApplyColorOnDamaged ();

		if(attacked)
		{
			//playerAnimator.ResetTrigger ("Attack");
			attacked = false;
		}

		if (Input.GetKey (KeyCode.Mouse0) || Input.GetKey (KeyCode.E))
		{
			attacked = true;
			//playerAnimator.SetTrigger("Attack");
		}
		if (Input.GetKey (KeyCode.A))
		{
			playerMovement.MoveLeft ();
		}
		if (Input.GetKey (KeyCode.D))
		{
			playerMovement.MoveRight ();
		}
		if (Input.GetKeyDown (KeyCode.Space))
		{
			playerMovement.Jump ();
		}

	}
}
