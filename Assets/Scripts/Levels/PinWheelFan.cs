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
        private Vector2 _force;
        private PlayerController _player;

        private void Awake()
        {
            CalculateForce(_windForce);
            if (_force.x != 0)
                _force += new Vector2(0, _upwardsForce);

        }

        private void LateUpdate()
        {
            if (_player?.GetCurrentState().GetType() != typeof(ParachutingState))
                _player = null;


            if (_player != null)
            {
                _player.rb.AddForce(_force);
            }
        }

        private void CalculateForce(float _windValue)
        {
            _force = (transform.right * _windValue);
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


