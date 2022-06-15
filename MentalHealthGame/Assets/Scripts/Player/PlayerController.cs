using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    public Animator animator;
    public Camera _followCamera;

    private Vector3 _playerVelocity;
    private Vector3 currentSpeed;

    public int jumps;

    public float _playerSpeed = 5f;
    public float defaultSpeed;
    public float sprintSpeed;
    public float _rotationSpeed = 10f;
    public float _jumpHeight = 1.0f;
    public float _gravityValue = -9.81f;

    public bool isSprinting;
    public bool isAiming;
    private bool _groundedPlayer;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
        Sprinting();
        SetAnimations();
        Aiming();
        Shooting();
    }

    private void Movement()
    {
        _groundedPlayer = _controller.isGrounded;

        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, _followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;
        currentSpeed = movementDirection;

        _controller.Move(movementDirection * _playerSpeed * Time.deltaTime);



        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        if (isAiming)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _followCamera.transform.eulerAngles.y, transform.rotation.z);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumps++;

            if (jumps < 2)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
            }

            animator.SetTrigger("StartJump");
            animator.ResetTrigger("StartJump");
        }

        if (_groundedPlayer)
        {
            jumps = 0;
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void Aiming()
    {
        isAiming = Input.GetMouseButton(1);
    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0) && isAiming)
        {
            FindObjectOfType<PlayerAttacks>().Shoot();
        }
    }

    private void SetAnimations()
    {
        animator.SetFloat("MoveSpeed", currentSpeed.magnitude);
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsAiming", isAiming);
    }

    private void Sprinting()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isSprinting)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) && isSprinting && currentSpeed != Vector3.zero)
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            _playerSpeed = sprintSpeed;
        }
        else
        {
            _playerSpeed = defaultSpeed;
        }
    }
}