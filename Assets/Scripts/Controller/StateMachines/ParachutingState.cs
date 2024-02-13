using Managers;
using System.Diagnostics;
using System.Numerics;

namespace Controller.StateMachines
{
    public class ParachutingState : Parachuting
    {
        public ParachutingState(PlayerController player) : base("ParachutingState", player)
        {
        }

        public override void EnterState()
        {
            // Set the falling animation.
            //temp for this state
            player.animator.SetBool(Falling, true);
            player.canParachute = false;

            player.rb.velocity = new UnityEngine.Vector2(0,0);
            //resets umbrella timer

        }
        public override void HandleInput()
        {
            base.HandleInput();

            if (player.IsGrounded())
                player.ChangeState(input.IsMoving ? player.walkingState : player.idleState);
            if (!InputManager.Jump.IsPressed())
                player.ChangeState(player.fallingState);
            

        }
        public override void ExitState()
        {
            // Set the falling animation to false.
            player.animator.SetBool(Falling, false);
        }
    }

}



