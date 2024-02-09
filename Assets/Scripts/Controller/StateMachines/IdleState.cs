namespace Controller.StateMachines
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
    }
}