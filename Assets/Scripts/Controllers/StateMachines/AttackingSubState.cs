using Managers;

namespace Controllers.StateMachines
{
    public class AttackingSubState : BaseSubState
    {
        public AttackingSubState(BaseState parentState) : base("AttackingSubState", parentState)
        {
        }

        public override void EnterSubState()
        {
            player.pieController.Charge();
            input.IsAttacking = true;
            
            player.animator.SetBool(BaseState.Attacking, true);
        }
        
        public override void UpdateSubState()
        {
            if (InputManager.Attack.IsPressed())
                player.pieController.Charging();
            
            InputManager.Attack.canceled += _ =>
            {
                player.pieController.HandlePieThrow();
                input.IsAttacking = false;
            };
        }

        public override void ExitSubState()
        {
            player.animator.SetBool(BaseState.Attacking, false);
        }
    }
}