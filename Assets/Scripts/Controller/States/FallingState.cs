using UnityEngine;

namespace Controller.States
{
    public class FallingState : Airborne
    {
        public FallingState(PlayerController player) : base("FallingState", player)
        {
        }
        
        public override void EnterState()
        {
            // Set the falling animation.
            player.animator.SetBool(Falling, true);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (player.IsGrounded())
                player.ChangeState(input.IsMoving ? player.walkingState : player.idleState);
        }
        
        public override void ExitState()
        {
            // Set the falling animation to false.
            player.animator.SetBool(Falling, false);
        }
    }
}