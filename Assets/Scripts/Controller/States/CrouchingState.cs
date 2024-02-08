using UnityEngine;

namespace Controller.States
{
    public class CrouchingState : PlayerState
    {
        public CrouchingState(PlayerController player) : base(player)
        {
        }
        
        public override void EnterState()
        {
            // player.animator.SetBool(Crouching, true);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (!isCrouching)
                player.ChangeState(player.idleState);
        }
        
        public override void FixedUpdateState()
        {
            player.rb.velocity = new Vector2(
                moveInput.x * 
                (player.playerSettings.movementSpeed * player.playerSettings.crouchSpeedMultiplier), 
                player.rb.velocity.y);
        }
        
        public override void ExitState()
        {
            // player.animator.SetBool(Crouching, false);
        }
    }
}