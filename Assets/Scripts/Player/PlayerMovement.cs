using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;

    private Rigidbody _playerRigidBody;


    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void MoveRight()
    {
        _playerRigidBody.AddForce(new Vector3(_speed, 0, 0), ForceMode.Impulse);
    }

    public void MoveLeft()
    {
        _playerRigidBody.AddForce(new Vector3(-_speed, 0, 0), ForceMode.Impulse);
    }

    private void Jump()
    {
        _playerRigidBody.AddForce(new Vector3(0, _jumpSpeed, 0), ForceMode.Impulse);
    }
}
