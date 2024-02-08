using Managers;
using UnityEngine;

namespace Controller.States
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player)
        {
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (isMoving)
                player.ChangeState(player.walkingState);
            else if (InputManager.Jump.IsInProgress())
                player.ChangeState(player.jumpingState);
        }
        
        public override void FixedUpdateState()
        {
            if (moveInput == Vector2.zero)
                player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        }
    }
}