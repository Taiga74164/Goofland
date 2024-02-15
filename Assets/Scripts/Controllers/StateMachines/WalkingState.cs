using UnityEngine;

namespace Controllers.StateMachines
{
    public class WalkingState : Grounded
    {
        public WalkingState(PlayerController player) : base("WalkingState", player)
        {
        }
        
        public override void EnterState()
        {
            // Set the walking animation.
            player.animator.SetBool(Walking, true);
            
            // Configure the audio source
            player.audioSource.Configure(player.playerSettings.walkSoundData);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (input.IsRunning)
                player.ChangeState(player.runningState);
            else if (input.IsIdle)
                player.ChangeState(player.idleState);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            // Move the player.
            player.rb.velocity = new Vector2(
                input.MoveInput.x *
                player.playerSettings.movementSpeed, 
                player.rb.velocity.y);
            
            // Play the walking sound.
            if (!input.IsJumping && player.IsGrounded() &&!player.audioSource.isPlaying)
                player.audioSource.Play();
        }
        
        public override void ExitState()
        {
            // Set the walking animation to false.
            player.animator.SetBool(Walking, false);
            
            // Stop the walking sound.
            player.audioSource.Stop();
        }
    }
}