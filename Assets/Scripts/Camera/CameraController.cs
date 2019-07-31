using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private bool _smoothYAxis = true;
    [SerializeField] private bool _smoothXAxis = true;

    void Awake()
    {

    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPosition = _target.position + _cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;

        if (!_smoothXAxis)
            transform.position = new Vector3(desiredPosition.x, transform.position.y, transform.position.z);

        if(!_smoothYAxis)
            transform.position = new Vector3(transform.position.x, desiredPosition.y, transform.position.z);
    }
}
