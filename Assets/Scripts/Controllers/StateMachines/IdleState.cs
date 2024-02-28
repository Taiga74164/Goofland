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
        }
        public override void UpdateState()
        {
            base.UpdateState();
            if (!player.IsGrounded())
                player.ChangeState(player.fallingState);
        }
    }
}