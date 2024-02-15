using UnityEngine;
using Controllers;
using Controllers.StateMachines;

namespace Levels
{
    public class PinWheelFan : MonoBehaviour
    {
        [SerializeField] private float windForce = 5.0f; 
        
        [Tooltip("Only used if fan is vertical.")]
        [SerializeField] private float upwardsForce = 5.0f; 

        private Vector2 _force; // Force that will be added to the player.
        private PlayerController _player;

        private void Awake()
        {
            CalculateForce(windForce);
        }

        /// <summary>
        /// Force will only be applied if player is parachuting
        /// </summary>
        private void FixedUpdate()
        {
            if (_player?.GetCurrentState().GetType() == typeof(ParachutingState))
                _player!.rb.AddForce(_force);
            else
                _player = null;
        }

        /// <summary>
        /// Calculates force based on direction fan is facing and the inputed value.
        /// </summary>
        /// <param name="windValue">Value of the wind force</param>
        private void CalculateForce(float windValue)
        {
            _force = (transform.right * windValue);
            if (_force.x != 0)
                _force += new Vector2(0, upwardsForce);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                _player = collision.gameObject.GetComponent<PlayerController>();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                _player = null;
        }
    }
}