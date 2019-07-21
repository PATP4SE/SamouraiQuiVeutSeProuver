using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float smoothSpeed;

    void Awake()
    {

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPosition = _target.position + _cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        //transform.LookAt(_target);
    }
}
