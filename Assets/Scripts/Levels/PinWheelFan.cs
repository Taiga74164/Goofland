using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using Controller.StateMachines;
using Unity.VisualScripting;

namespace Levels
{
    public class PinWheelFan : MonoBehaviour
    {
        [SerializeField] private float _windForce; 
        
        [Tooltip("only used if fan is vertical")]
        [SerializeField] private float _upwardsForce;

        private Vector2 _force; //force that will be added to the player
        private PlayerController _player;

        private void Awake()
        {
            CalculateForce(_windForce);
        }

        private void LateUpdate() //force will only be applied if player is parachuting
        {
            if (_player?.GetCurrentState().GetType() != typeof(ParachutingState))
                _player = null;

            if (_player != null)
            {
                _player.rb.AddForce(_force);
            }
        }

        /// <summary>
        /// calculates force based on direction fan is facing and the inputed value
        /// </summary>
        /// <param name="_windValue"></param>
        private void CalculateForce(float _windValue)
        {
            _force = (transform.right * _windValue);
            if (_force.x != 0)
                _force += new Vector2(0, _upwardsForce);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _player = collision.gameObject.GetComponent<PlayerController>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                _player = null;
        }

        


    }
}


