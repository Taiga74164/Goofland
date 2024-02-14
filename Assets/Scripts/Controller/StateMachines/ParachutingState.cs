using Managers;
using UnityEngine;

namespace Controller.StateMachines
{
    public class ParachutingState : Airborne
    {
        public ParachutingState(PlayerController player) : base("ParachutingState", player)
        {
        }

        public override void EnterState()
        {
            // Set the falling animation.
            player.animator.SetBool(Falling, true);
            player.CanParachute = false;

            // Prevents player velocity from exceeding the slowdown of the glide.
            player.rb.velocity = Vector2.zero;
        }
        
        public override void HandleInput()
        {
            base.HandleInput();

            if (player.IsGrounded())
                player.ChangeState(input.IsMoving ? player.walkingState : player.idleState);
            
            if (!InputManager.Jump.IsPressed())
                player.ChangeState(player.fallingState);
        }
        
        protected override void HandleClampFallSpeed()
        {
            // Apply gravity diminisher 
            player.rb.velocity += Vector2.up * (Physics2D.gravity.y *
                                                (player.playerSettings.umbrellaGravityDiminisher - 1) * Time.deltaTime);
            input.IsJumping = false;   
        }
        
        public override void ExitState()
        {
            // Set the falling animation to false.
            player.animator.SetBool(Falling, false);
        }
    }
}



