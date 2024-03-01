using Managers;

namespace Controllers.StateMachines
{
    /// <summary>
    /// Hierarchy class for grounded player states.
    /// </summary>
    public class Grounded : BaseState
    {
        private bool _isInvincible;
        private float _invincibilityFrameCounter;
        
        protected Grounded(string name, PlayerController player) : base(name, player)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            
            InputManager.Jump.started += _ => player.ChangeState(player.jumpingState);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            // if (input.IsCrouching)
            //     player.ChangeState(player.crouchingState);
            
            if (input.IsFalling)
                player.ChangeState(player.fallingState);
            
            if (InputManager.Attack.triggered)
                ChangeSubState(new AttackingSubState(this));
            
            if (!input.IsAttacking)
                ChangeSubState(null);
        }
        
        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            
            if (player.CanJump())
                player.ChangeState(player.jumpingState);
        }
    }
}