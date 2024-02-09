using UnityEngine;

namespace Controller.States
{
    public class RunningState : Grounded
    {
        public RunningState(PlayerController player) : base("RunningState", player)
        {
        }
        
        public override void EnterState()
        {
            // Set the running animation.
            player.animator.SetBool(Running, true);
            
            // Configure the audio source
            player.audioSource.Configure(player.playerSettings.runSoundData);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (!input.IsRunning)
                player.ChangeState(player.walkingState);
            else if (input.IsIdle)
                player.ChangeState(player.idleState);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            // Move the player.
            player.rb.velocity = new Vector2(
                input.MoveInput.x * 
                (player.playerSettings.movementSpeed * player.playerSettings.runSpeedMultiplier), 
                player.rb.velocity.y);
            
            // Play the running sound.
            if (!input.IsJumping && player.IsGrounded() &&!player.audioSource.isPlaying)
                player.audioSource.Play();
        }
        
        public override void ExitState()
        {
            // Set the running animation to false.
            player.animator.SetBool(Running, false);
            
            // Stop the running sound.
            player.audioSource.Stop();
        }
    }
}