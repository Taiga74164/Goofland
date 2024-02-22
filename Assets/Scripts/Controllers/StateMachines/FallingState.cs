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
        }
        
        public override void ExitState()
        {
            // Set the falling animation to false.
            player.beenWarped = false;
            player.animator.SetBool(Falling, false);
        }
        protected override void HandleClampFallSpeed()
        {
            base.HandleClampFallSpeed();
            switch (player.rb.velocity.y)
            {
                case > 0 when InputManager.Jump.IsPressed():
                    // Apply the low jump multiplier.
                    player.rb.velocity -= Vector2.down *
                                          (Physics2D.gravity.y * (player.playerSettings.fallMultiplier - 1) *
                                           Time.deltaTime);
                    input.IsJumping = false;
                    break;
                }
            }
        }
}