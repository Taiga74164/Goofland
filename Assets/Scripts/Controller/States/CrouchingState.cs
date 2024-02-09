using UnityEngine;

namespace Controller.States
{
    public class CrouchingState : PlayerState
    {
        public CrouchingState(PlayerController player) : base("CrouchingState", player)
        {
        }
        
        public override void EnterState()
        {
            // player.animator.SetBool(Crouching, true);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (!input.IsCrouching)
                player.ChangeState(player.idleState);
        }
        
        public override void UpdateState()
        {
            player.rb.velocity = new Vector2(
                input.MoveInput.x * 
                (player.playerSettings.movementSpeed * player.playerSettings.crouchSpeedMultiplier), 
                player.rb.velocity.y);
        }
        
        public override void ExitState()
        {
            // player.animator.SetBool(Crouching, false);
        }
    }
}