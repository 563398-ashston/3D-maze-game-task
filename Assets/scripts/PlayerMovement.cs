
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;

    public Transform orientation;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    [Header("Gravity")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        MyInput();
        MovePlayer();
        Jump();
    }

    private void MyInput()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * input.y + right * input.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
    }

    private void MovePlayer()
    {
        // Apply gravity
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        // Combine horizontal movement and gravity
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (jumpAction.action.WasPressedThisFrame() && controller.isGrounded)
        {
            verticalVelocity = jumpHeight;
        }
    }
}
