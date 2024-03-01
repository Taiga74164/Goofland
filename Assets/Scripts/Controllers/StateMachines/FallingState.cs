using UnityEngine;
using Managers;

namespace Controllers.StateMachines
{
    public class FallingState : Airborne
    {
        public FallingState(PlayerController player) : base("FallingState", player)
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

            if (input.IsParachuting)
                player.ChangeState(player.parachutingState);

            if (player.IsGrounded())
                player.ChangeState(input.IsMoving ? player.walkingState : player.idleState);
            
            if (player.rb.velocity.x > player.playerSettings.maxVelocity)
            {
                player.rb.velocity = new (player.playerSettings.maxVelocity, player.rb.velocity.y);
            }
        }
        
        public override void ExitState()
        {
            // Set the beenWarped flag to false.
            if (player.beenWarped)
                player.beenWarped = false;
            
            // Set the falling animation to false.
            player.animator.SetBool(Falling, false);
        }
        
        protected override void HandleClampFallSpeed()
        {
            base.HandleClampFallSpeed();
            switch (player.rb.velocity.y)
            {
                case > 0 when InputManager.Jump.IsPressed() && input.IsJumping:
                    // Apply the low jump multiplier.
                    player.YVelocity += Physics2D.gravity.y * (player.playerSettings.fallMultiplier - 1) * 
                                        Time.fixedDeltaTime;
                    input.IsJumping = false;
                    break;
            }
        }
    }
}