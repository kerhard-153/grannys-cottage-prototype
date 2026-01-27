using UnityEngine;
using System;
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float turnSpeed = 1080f;

    private InputSystem_Actions _playerInputActions;
    private Vector3 _moveInput;
    private CharacterController _characterController;

    public float playerHealth = 100f;

    private bool isJumping = false;

    private void Awake()
    {
        _playerInputActions = new InputSystem_Actions();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // enables Player action map in Input System
        _playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // disables Player action map in Input System
        _playerInputActions.Player.Disable();
    }

    private void Update()
    {
        GatherInput();

        Look();
        Move();
    }

    // causes the player to turn towards and face the direction _moveInput indicates
    private void Look()
    {
        // if no input, do nothing
        if (_moveInput == Vector3.zero) return;

        // This code block corrects our inputs to the isometric camera angle
        Matrix4x4 isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
        Vector3 multipliedMatrix = isometricMatrix.MultiplyPoint3x4(_moveInput);

        // turns player towards input direction
        Quaternion rotation = Quaternion.LookRotation(multipliedMatrix, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    private void GatherInput()
    {
        // gathers movement input
        Vector2 moveInput = _playerInputActions.Player.Move.ReadValue<Vector2>();
        if (!isJumping)
        {
            _moveInput = new Vector3(moveInput.x, 0, moveInput.y);    
        } 
        else if (isJumping && transform.position.y < 4 )
        {
            _moveInput = new Vector3(moveInput.x, 2, moveInput.y);
        }
        

        // gathers jump input
        if (_playerInputActions.Player.Jump.WasPressedThisFrame())
        {
            isJumping = true;
        }
        if (_playerInputActions.Player.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
        }
    }

   // Moves the player in the direction _moveInput indicates
    private void Move()
    {
        Vector3 moveDirection = transform.forward * runSpeed * _moveInput.magnitude * Time.deltaTime;
        _characterController.Move(moveDirection);
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
    }

    
    private void Jump()
    {
        
    }

}
