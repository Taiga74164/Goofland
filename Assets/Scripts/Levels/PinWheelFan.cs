using UnityEngine;
using Controllers;
using Controllers.StateMachines;

namespace Levels
{
    public class PinWheelFan : MonoBehaviour
    {
        float maxDistance = 3f;
        [SerializeField] private float windForce = 5.0f;

        [Tooltip("Only used if fan is vertical.")]
        [SerializeField] private float upwardsForce = 5.0f;

        [SerializeField] private bool isSideways;
        
        private Vector2 _force; // Force that will be added to the player.
        private PlayerController _player;

        private void Awake()
        {
            maxDistance = isSideways ? GetComponent<BoxCollider2D>().bounds.max.x : GetComponent<BoxCollider2D>().bounds.max.y;
            CalculateForce(windForce);
        }

        /// <summary>
        /// Force will only be applied if player is parachuting
        /// </summary>
        private void FixedUpdate()
        {
            if (_player && _player.GetCurrentState().GetType() == typeof(ParachutingState))
            {

                float distance = Mathf.Min((_player.transform.position - transform.position).magnitude, maxDistance);
                distance = 1.0f - distance / maxDistance;
                
                CalculateForce(windForce, distance);
                _player!.rb.AddForce(_force);
                _player.rb.velocity = new Vector2(_player.rb.velocity.x, 0);
            }

        }

        /// <summary>
        /// Calculates force based on direction fan is facing and the inputted value.
        /// </summary>
        /// <param name="windValue">Value of the wind force</param>
        private void CalculateForce(float windValue, float distance = 1.0f)
        {
            _force = (transform.right * windValue) * distance;
            if (isSideways)
            {
                var smoothDistance = Mathf.SmoothStep(0, 1, distance);
                _force = transform.right * windValue * smoothDistance;
            }
            else
            {
                // The fan is vertical, apply the upwards force.
                _force += Vector2.up * upwardsForce * distance;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.IsPlayer())
                _player = collision.gameObject.GetComponent<PlayerController>();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.IsPlayer())
                _player = null;
        }
    }
}