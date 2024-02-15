using UnityEngine;

namespace Controllers.StateMachines
{
    public class JumpingState : Airborne
    {
        public JumpingState(PlayerController player) : base("JumpingState", player)
        {
        }
        
        public override void EnterState()
        {
            input.IsJumping = true;
            
            player.CanParachute = true;

            // Set the jumping animation.
            player.animator.SetBool(Jumping, true);
            
            Jump();
            
            // Configure the audio source and play the jump sound.
            player.audioSource.Configure(player.playerSettings.jumpSoundData);
            player.audioSource.Play();
        }

        private void Jump()
        {
            if (!player.IsGrounded() && player.CoyoteTimeCounter <= 0.0f) return;
            
            // Increase the jump force if the player is running.
            var jumpForce = player.playerSettings.jumpHeight;
            if (input.IsRunning)
                jumpForce *= player.playerSettings.jumpBoostMultiplier;
            
            player.rb.velocity = new Vector2(player.rb.velocity.x, jumpForce);
            player.ResetCoyoteTimeAndJumpBufferCounter();
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (player.rb.velocity.y < Mathf.Epsilon)
                player.ChangeState(player.fallingState);
            
            if (input.IsParachuting)
                player.ChangeState(player.parachutingState);
        }
        
        public override void ExitState()
        {
            // Set the jumping animation to false.
            player.animator.SetBool(Jumping, false);
            
            // Stop the jump sound.
            player.audioSource.Stop();
            
            input.IsJumping = false;
        }
    }
}