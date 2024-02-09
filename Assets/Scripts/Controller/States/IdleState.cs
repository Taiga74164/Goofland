using Managers;

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
            
            if (input.IsMoving)
                player.ChangeState(player.walkingState);
            else if (InputManager.Jump.IsInProgress())
                player.ChangeState(player.jumpingState);
        }
    }
}