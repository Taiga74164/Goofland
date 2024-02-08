using Managers;
using UnityEngine;

namespace Controller.States
{
    public class WalkingState : PlayerState
    {
        public WalkingState(PlayerController player) : base(player)
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
            
            if (isRunning)
                player.ChangeState(player.runningState);
            else if (!isMoving && !isRunning && player.IsGrounded())
                player.ChangeState(player.idleState);
            else if (isCrouching)
                player.ChangeState(player.crouchingState);
            else if (InputManager.Jump.IsInProgress())
                player.ChangeState(player.jumpingState);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            // Play the walking sound.
            if (!isJumping && player.IsGrounded() &&!player.audioSource.isPlaying)
                player.audioSource.Play();
        }

        public override void FixedUpdateState()
        {
            // Move the player.
            player.rb.velocity = new Vector2(
                moveInput.x *
                player.playerSettings.movementSpeed, 
                player.rb.velocity.y);
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