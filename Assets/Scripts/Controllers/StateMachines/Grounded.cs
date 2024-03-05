using Managers;
using UnityEngine.InputSystem;

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

        public override void HandleInput()
        {
            base.HandleInput();
            
            
            if (input.IsFalling)
                player.ChangeState(player.fallingState);
            
            if (InputManager.Attack.triggered)
                ChangeSubState(new AttackingSubState(this));
            
            if (!input.IsAttacking)
                ChangeSubState(null);
        }
        
        public override void UpdateState()
        {
            base.UpdateState();
            
            if (player.CanJump())
                player.ChangeState(player.jumpingState);
        }
    }
}