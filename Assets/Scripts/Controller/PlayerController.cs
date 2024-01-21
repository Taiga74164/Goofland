using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float movementSpeed = 5.0f;
    public float runSpeed = 5.0f;
    public float jumpHeight = 5.0f;
    
    [Header("Ground Check Settings")]
    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayerMask;
    
    #region Input Actions
    
    private InputAction _move, _jump, _run;
    
    #endregion

    private Rigidbody2D _rb;
    private Vector2 _moveInput = Vector2.zero;
    private bool _isMoving, _isRunning;

    private void Start()
    {
        // Get the rigidbody component.
        _rb = GetComponent<Rigidbody2D>();
        
        // Set up input action references.
        _move = InputManager.move;
        _jump = InputManager.jump;
        _run = InputManager.run;
        
        // Listen for input actions.
        _jump.started += Jump;
        _run.started += Run;

    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Get the input values.
        _moveInput = _move.ReadValue<Vector2>();
        // Update the player's state.
        _isMoving = _moveInput != Vector2.zero;
        // Update the player's running state.
        _isRunning = _run.ReadValue<float>() is 1 ? true : false;
        
        // Move the player.
        _rb.velocity = new Vector2(_moveInput.x * (movementSpeed + (_isRunning ? runSpeed : 0)), _rb.velocity.y);
    }
    
    /// <summary>
    /// Invoked when the jump action is performed.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Jump(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;
        
        // Jump.
        _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
    }
    
    /// <summary>
    /// Invoked when the Run action is performed.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Run(InputAction.CallbackContext context)
    {
        //
    }
    
    private bool IsGrounded() => Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
}
