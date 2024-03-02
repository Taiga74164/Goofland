using Managers;
using UnityEngine;

namespace Controllers.StateMachines
{
    /// <summary>
    /// Hierarchical class for airborne player states.
    /// </summary>
    public class Airborne : Grounded
    {
        protected float velocityY;
        protected Airborne(string name, PlayerController player) : base(name, player)
        {
        }

        public override void HandleInput()
        {
            if (InputManager.Attack.triggered)
                ChangeSubState(new AttackingSubState(this));
            
            if (!input.IsAttacking)
                ChangeSubState(null);
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();

            Move();
            HandleClampFallSpeed();
            // player.rb.velocity = new Vector2(player.rb.velocity.x, velocityY);
        }
        
        private void Move()
        {
            if (player.IsKnockback) return;
            
            // Move the player while in the air.
            if (!player.beenWarped)
            {
                player.rb.velocity = new Vector2(
                input.MoveInput.x *
                player.playerSettings.movementSpeed,
                player.rb.velocity.y);
            }
            else
            {
                var playerVelocity =  new Vector2(player.rb.velocity.x +
                input.MoveInput.x *
                player.playerSettings.movementSpeed,
                player.rb.velocity.y);
                //if(playerVelocity.magnitude <= player.rb.velocity.magnitude)
                  //  player.rb.velocity = playerVelocity;
            }
        }

        protected virtual void HandleClampFallSpeed()
        {
            switch (player.rb.velocity.y)
            {
                // If the player is falling, increase the fall speed.
                case < 0:
                    // Apply the fall multiplier.
                    player.YVelocity += Physics2D.gravity.y * (player.playerSettings.fallMultiplier - 1) * 
                                        Time.fixedDeltaTime;
                    input.IsJumping = false;
                    break;
                // If the player is jumping and the jump button is released, decrease the jump speed.
                case > 0 when !InputManager.Jump.IsPressed():
                    // Apply the low jump multiplier.
                    player.YVelocity += Physics2D.gravity.y * (player.playerSettings.lowJumpMultiplier - 1) * 
                                        Time.fixedDeltaTime;
                    break;
                default:
                    player.YVelocity +=  Physics2D.gravity.y * Time.fixedDeltaTime;
                    break;
            }
        }
    }
}