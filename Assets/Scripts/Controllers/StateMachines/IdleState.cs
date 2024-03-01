using UnityEngine;

namespace Controllers.StateMachines
{
    public class IdleState : Grounded
    {
        public IdleState(PlayerController player) : base("IdleState", player)
        {
        }

        public override void HandleInput()
        {
            base.HandleInput();
            
            if (input.IsMoving)
                player.ChangeState(player.walkingState);
            
            if (!player.IsGrounded())
                player.ChangeState(player.fallingState);
        }
        
        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            
            player.rb.velocity = new Vector2(0 , player.rb.velocity.y);
        }
    }
}