using UnityEngine;

public class PlayerController : MonoBehaviour {

	//[SerializeField] private Animator playerAnimator;

    [SerializeField] private CharacterMovement2D _characterMovement;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Camera _camera;

    private bool _hasAttacked;
    private bool _isBlocking;
    private PlayerMovement _playerMovement;
    private Player _player;
    private Vector3 _mousePosition;


    private float _horizontalDirection;
    private bool _isJumping;

    private const float MOUSE_POSITION_Z = 3;

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
            _playerAnimator.SetBool("IsWalking", true);
            Debug.Log("isWalking");
        }    
        else
        {
            Debug.Log("notWalking");
            _playerAnimator.SetBool("IsWalking", false);
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

        if (Input.GetButtonDown("Block"))
        {
            _isBlocking = true;
        }

        if (Input.GetButtonUp("Block"))
        {
            _isBlocking = false;
        }

        Vector3 direction = MousePosition - _playerTransform.position;

        RaycastHit2D[] highRay = Physics2D.RaycastAll(_playerTransform.position, direction);

        if (_isBlocking)
        {
            Debug.Log("Blocking");
            Debug.DrawRay(_playerTransform.position, direction, Color.blue);
        }
        else
        {
            Debug.DrawRay(_playerTransform.position, direction, Color.white);
        }


    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(MousePosition, 0.1f);
    }

    private void FixedUpdate()
    {
        _characterMovement.Move(_horizontalDirection * Time.fixedDeltaTime, false, _isJumping);
        _isJumping = false;
    }

    private Vector3 MousePosition
    {
        get
        {
            _mousePosition = Input.mousePosition;
            _mousePosition.z = MOUSE_POSITION_Z;
            _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
            return _mousePosition;
        }
    }
}
