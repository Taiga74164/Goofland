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

        private Vector2 _force; // Force that will be added to the player.
        private PlayerController _player;

        private void Awake()
        {

            maxDistance = GetComponent<BoxCollider2D>().bounds.max.y;// Mathf.Max(, GetComponent<BoxCollider2D>().bounds.max.x);
            Debug.LogFormat("Max distance is {0}", maxDistance);
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
                Debug.LogFormat("Distance is initially {0}", distance);
                distance = 1.0f - distance / maxDistance;
                Debug.LogFormat("Distance is {0}", distance);
                CalculateForce(windForce, distance);
                _player!.rb.AddForce(_force);
            }

        }

        /// <summary>
        /// Calculates force based on direction fan is facing and the inputted value.
        /// </summary>
        /// <param name="windValue">Value of the wind force</param>
        private void CalculateForce(float windValue, float distance = 1.0f)
        {
            _force = (transform.right * windValue) * distance;
            if (_force.x != 0)
                _force += new Vector2(0, upwardsForce) * distance;
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