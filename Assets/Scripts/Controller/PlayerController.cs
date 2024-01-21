using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float movementSpeed = 5.0f;
    
    #region Input Actions
    
    private InputAction _move, _jump, _run;
    
    #endregion

    private Rigidbody2D _rb;
    private Vector2 _moveInput = Vector2.zero;
    private bool _isMoving;
    
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
        
        // Move the player.
        _rb.velocity = new Vector2(_moveInput.x * movementSpeed, _rb.velocity.y);
    }
    
    /// <summary>
    /// Invoked when the jump action is performed.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Jump(InputAction.CallbackContext context)
    {
        
    }
    
    /// <summary>
    /// Invoked when the Run action is performed.
    /// </summary>
    /// <param name="context">The input context.</param>
    private void Run(InputAction.CallbackContext context)
    {
        
    }
}
