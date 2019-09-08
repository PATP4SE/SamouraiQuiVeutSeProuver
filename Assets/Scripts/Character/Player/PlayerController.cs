using UnityEngine;

public class PlayerController : MonoBehaviour {

	//[SerializeField] private Animator playerAnimator;

    [SerializeField] private CharacterMovement2D _characterMovement;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Camera _camera;

    private bool _hasAttacked;
    private PlayerMovement _playerMovement;
    private Player _player;
    private Vector3 _mousePosition1;
    private Vector3 _mousePosition2;
    private Vector3 _mousePosition3;


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

        Vector3 mousePosition1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition1.z = 0;
        Vector3 mousePosition2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition2.z = 0;
        Vector3 mousePosition3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition3.z = 0;

        _mousePosition1 = mousePosition1;

        //Vector3 direction = new Vector3(
        //    mousePosition.x - _playerTransform.position.x,
        //    mousePosition.y - _playerTransform.position.y);

        Debug.Log("Mouse position: " + mousePosition1.x + " x " + mousePosition1.y + " y ");

        Ray ray = new Ray(_playerTransform.position, Input.mousePosition);
        Debug.DrawRay(_playerTransform.position, mousePosition1 - _playerTransform.position);

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_mousePosition1, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_mousePosition1, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_mousePosition1, 0.1f);
    }

    private void FixedUpdate()
    {
        _characterMovement.Move(_horizontalDirection * Time.fixedDeltaTime, false, _isJumping);
        _isJumping = false;
    }
}
