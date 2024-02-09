using UnityEngine;

namespace Controller.States
{
    public class FallingState : PlayerState
    {
        public FallingState(PlayerController player) : base(player)
        {
        }
        
        public override void EnterState()
        {
            // Set the falling animation.
            player.animator.SetBool(Falling, true);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (player.IsGrounded())
                player.ChangeState(input.IsMoving ? player.walkingState : player.idleState);
        }
        
        public override void UpdateState()
        {
            // Move the player while in the air.
            var horizontalSpeed = input.MoveInput.x * player.playerSettings.movementSpeed;
            player.rb.velocity = new Vector2(horizontalSpeed, player.rb.velocity.y);
            
            // If the player is falling, increase the fall speed.
            player.rb.velocity += 
                Vector2.up * 
                (Physics2D.gravity.y * (player.playerSettings.fallMultiplier - 1) * 
                 Time.deltaTime);
        }
        
        public override void ExitState()
        {
            // Set the falling animation to false.
            player.animator.SetBool(Falling, false);
        }
    }
}