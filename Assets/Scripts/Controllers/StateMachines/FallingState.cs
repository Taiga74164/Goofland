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
            switch (player.rb.velocity.y)
            {
                // If the player is falling, increase the fall speed.
                case < 0:
                   
                    // Apply the fall multiplier.
                    player.rb.velocity += Vector2.up *
                                          (Physics2D.gravity.y * (player.playerSettings.fallMultiplier - 1) *
                                           Time.deltaTime);
                    input.IsJumping = false;
                    break;
                // If the player is jumping and the jump button is released, decrease the jump speed.
                case > 0 when !InputManager.Jump.IsPressed():
                    // Apply the low jump multiplier.
                    player.rb.velocity += Vector2.up *
                                          (Physics2D.gravity.y * (player.playerSettings.lowJumpMultiplier - 1) *
                                           Time.deltaTime);
                    break;

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