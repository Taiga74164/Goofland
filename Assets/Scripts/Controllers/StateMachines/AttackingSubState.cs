namespace Controllers.StateMachines
{
    public class AttackingSubState : BaseSubState
    {
        public AttackingSubState(BaseState parentState) : base("AttackingSubState", parentState)
        {
        }

        public override void EnterSubState()
        {
            input.IsAttacking = true;
            
            // Set the attacking animation to true.
            player.animator.SetBool(BaseState.Attacking, true);
        }

        public override void UpdateSubState()
        {
            if (input.IsAttacking)
            {
                player.pieController.HandlePieThrow();
                input.IsAttacking = false;
            }
        }

        public override void ExitSubState()
        {
            // Set the attacking animation to false.
            player.animator.SetBool(BaseState.Attacking, false);
        }
    }
}