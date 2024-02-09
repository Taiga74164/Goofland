using Managers;
using UnityEngine;

namespace Controller.States
{
    public class JumpingState : PlayerState
    {
        public JumpingState(PlayerController player) : base(player)
        {
        }
        
        public override void EnterState()
        {
            input.IsJumping = true;
            
            // Set the jumping animation.
            player.animator.SetBool(Jumping, true);
            
            Jump();
            
            // Configure the audio source and play the jump sound.
            player.audioSource.Configure(player.playerSettings.jumpSoundData);
            player.audioSource.Play();
        }
        
        private void Jump()
        {
            // Increase the jump force if the player is running.
            var jumpForce = player.playerSettings.jumpHeight;
            if (input.IsRunning)
            {
                jumpForce *= player.playerSettings.jumpBoostMultiplier;
                Debug.Log("Jump Boost!");
            }
            
            player.rb.velocity = new Vector2(player.rb.velocity.x, jumpForce);
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            if (player.rb.velocity.y < 0.0f)
                player.ChangeState(player.fallingState);
        }
        
        public override void UpdateState()
        {
            // Move the player while in the air.
            var horizontalSpeed = input.MoveInput.x * player.playerSettings.movementSpeed;
            player.rb.velocity = new Vector2(horizontalSpeed, player.rb.velocity.y);
            
            // If the player is falling, increase the fall speed.
            if (player.rb.velocity.y > 0.0f && !InputManager.Jump.IsPressed())
                player.rb.velocity += 
                    Vector2.up * 
                    (Physics2D.gravity.y * (player.playerSettings.lowJumpMultiplier - 1) * 
                     Time.deltaTime);
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