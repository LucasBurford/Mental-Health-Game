using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{   
    public Transform _target;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    public Vector2 _rotationXMinMax = new Vector2(-40, 40);

    public float xOffset;
    public float yOffset;
    public float zOffset;

    public float _mouseSensitivity = 3.0f;
    public float _distanceFromTarget = 3.0f;
    public float _smoothTime = 0.2f;
    private float _rotationY;
    private float _rotationX;

    public bool invertY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        if (invertY)
        {
            _rotationY += mouseX;
            _rotationX += mouseY;
        }
        else
        {
            _rotationY += mouseX;
            _rotationX -= mouseY;
        }

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = new Vector3(_target.position.x + xOffset, _target.position.y + yOffset, _target.position.z + zOffset) - transform.forward * _distanceFromTarget;
    }
}