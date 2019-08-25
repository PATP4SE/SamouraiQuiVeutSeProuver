using UnityEngine;

public class PlayerController : MonoBehaviour {

	//[SerializeField] private Animator playerAnimator;

    [SerializeField] private CharacterMovement2D _characterMovement;
    [SerializeField] private Animator _playerAnimator;

    private bool _hasAttacked;
    private PlayerMovement _playerMovement;
    private Player _player;

    private float _horizontalDirection;
    private bool _isJumping;

    // Use this for initialization
    void Start ()
    {
        Rigidbody2D playerRigidBody = GetComponent<Rigidbody2D> ();
		_playerMovement = GetComponent<PlayerMovement> ();
        _player = GetComponent<Player> ();

        _playerMovement.StartMovement ();
	}

	// Update is called once per frame
	void Update ()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");

        if (_horizontalDirection != 0)
        {
            _playerAnimator.SetBool("isWalking", true);
            Debug.Log("isWalking");
        }    
        else
        {
            _playerAnimator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            _playerAnimator.SetTrigger("Jump");
            _isJumping = true;
        }

        if (_hasAttacked)
		{
			_playerAnimator.ResetTrigger ("Attack");
            _hasAttacked = false;
		}

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.E))
        {
            _hasAttacked = true;
            _playerAnimator.SetTrigger("Attack");
        }

        //if (Input.GetKey(KeyCode.A))
        //{
        //	playerMovement.MoveLeft ();
        //}
        //if (Input.GetKey (KeyCode.D))
        //{
        //	playerMovement.MoveRight ();
        //}
        //if (Input.GetKeyDown (KeyCode.Space))
        //{
        //	playerMovement.Jump ();
        //}

    }

    private void FixedUpdate()
    {
        _characterMovement.Move(_horizontalDirection * Time.fixedDeltaTime, false, _isJumping);
        _isJumping = false;
    }
}
