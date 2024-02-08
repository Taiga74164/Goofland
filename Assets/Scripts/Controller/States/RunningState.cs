using Managers;
using UnityEngine;

namespace Controller.States
{
    public class RunningState : PlayerState
    {
        public RunningState(PlayerController player) : base(player)
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
            
            if (!isRunning)
                player.ChangeState(player.walkingState);
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
            
            // Play the running sound.
            if (!isJumping && player.IsGrounded() &&!player.audioSource.isPlaying)
                player.audioSource.Play();
        }

        public override void FixedUpdateState()
        {
            player.rb.velocity = new Vector2(
                moveInput.x * 
                (player.playerSettings.movementSpeed * player.playerSettings.runSpeedMultiplier), 
                player.rb.velocity.y);
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