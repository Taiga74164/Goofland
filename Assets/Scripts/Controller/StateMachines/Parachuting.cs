using Managers;
using UnityEngine;


namespace Controller.StateMachines
{
    public class Parachuting : Airborne
    {
        protected Parachuting(string name, PlayerController player) : base(name, player)
        {
        }
        

        public override void UpdateState()
        {
            base.UpdateState();
        }
        protected override void HandleClampFallSpeed()
        {
            //apply GravityDiminisher 
            player.rb.velocity += Vector2.up * (Physics2D.gravity.y * (player.playerSettings.umbrellaGravityDiminisher - 1) * Time.deltaTime);
            input.IsJumping = false;   
        }

    }
}


