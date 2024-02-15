using Managers;

namespace Controllers.StateMachines
{
    /// <summary>
    /// Hierarchy class for grounded player states.
    /// </summary>
    public class Grounded : BaseState
    {
        protected Grounded(string name, PlayerController player) : base(name, player)
        {
        }
        
        public override void HandleInput()
        {
            base.HandleInput();

            if (input.IsCrouching)
                player.ChangeState(player.crouchingState);
            
            if (InputManager.Jump.IsInProgress())
                player.ChangeState(player.jumpingState);

            if (input.IsFalling)
                player.ChangeState(player.fallingState);
            
            if (InputManager.Attack.triggered)
                ChangeSubState(new AttackingSubState(this));
        }
        
        public override void UpdateState()
        {
            base.UpdateState();
            
            if (player.CanJump())
                player.ChangeState(player.jumpingState);
            
            currentSubState?.UpdateSubState();
        }
    }
}