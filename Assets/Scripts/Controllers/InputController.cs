using Managers;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Centralized input controller.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        private PlayerController _playerController;
        
        public Vector2 MoveInput { get; private set; }
        
        public bool IsIdle { get; private set; }
        
        public bool IsMoving { get; private set; }
        
        public bool IsRunning { get; private set; }
        
        public bool IsCrouching { get; private set; }
        
        public bool IsJumping { get; set; }
        
        public bool IsFalling { get; private set; }
        
        public bool IsAttacking { get; set; }
        
        public bool IsParachuting { get; private set; }
        
        private void Awake() => _playerController = GetComponent<PlayerController>();
        
        private void Update()
        {
            // Get the input values.
            MoveInput = InputManager.Move.ReadValue<Vector2>();
            
            // Update the player's idle state.
            IsIdle = !IsMoving && !IsRunning && _playerController.IsGrounded();
            
            // Update the player's state.
            IsMoving = MoveInput != Vector2.zero || InputManager.Move.IsInProgress();
            
            // Update the player's running state.
            IsRunning = InputManager.Run.IsPressed() && IsMoving;
            
            // Update the player's crouching state.
            IsCrouching = InputManager.Crouch.IsPressed() && _playerController.IsGrounded();
            
            // Update the player's falling state.
            IsFalling = _playerController.rb.velocity.y < -0.2f;
            
            // Update the player's parachuting state.
            IsParachuting = InputManager.Jump.triggered && _playerController.CanParachute;
        }
    }
}