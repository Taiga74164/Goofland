using Managers;
using UnityEngine;

namespace Controller.States
{
    /// <summary>
    /// Abstract class for player states.
    /// </summary>
    public abstract class PlayerState
    {
        protected PlayerController player;
        protected PlayerState(PlayerController player)
        {
            this.player = player;
        }

        #region Inputs
        
        protected Vector2 moveInput = Vector2.zero;
        protected bool isMoving, isRunning, isCrouching, isJumping, isFalling, isAttacking;

        #endregion
        
        #region Cached Properties

        protected static readonly int Idle = Animator.StringToHash("Idle");
        protected static readonly int Walking = Animator.StringToHash("Walking");
        protected static readonly int Running = Animator.StringToHash("Running");
        protected static readonly int Jumping = Animator.StringToHash("Jumping");
        protected static readonly int Falling = Animator.StringToHash("Falling");
        protected static readonly int Attacking = Animator.StringToHash("Attacking");

        #endregion
        
        public virtual void EnterState() { }

        public virtual void HandleInput()
        {
            // Get the input values.
            moveInput = InputManager.Move.ReadValue<Vector2>();
            
            // Update the player's state.
            isMoving = moveInput != Vector2.zero;
            
            // Update the player's running state.
            isRunning = InputManager.Run.IsPressed() && isMoving;
            
            // Update the player's crouching state.
            isCrouching = InputManager.Crouch.IsPressed() && player.IsGrounded();
            
            // Update the player's falling state.
            isFalling = player.rb.velocity.y < 0.0f;
            
            // Update the player's jumping state.
            // isJumping = InputManager.Jump.WasPressedThisFrame() && player.IsGrounded();
        }

        public virtual void UpdateState()
        {
            // Update the player's sprite based on the direction they are facing.
            var animatorTransform = player.animator.transform;
            animatorTransform.eulerAngles = moveInput.x switch
            {
                // right
                > 0 => Vector3.zero,
                // left
                < 0 => new Vector3(0, 180, 0),
                _ => animatorTransform.eulerAngles
            };
        }

        public virtual void FixedUpdateState() { }
        
        public virtual void ExitState() { }
    }
}
