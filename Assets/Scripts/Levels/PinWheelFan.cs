using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;

namespace Levels
{
    public class PinWheelFan : MonoBehaviour
    {
        [SerializeField] private float _windForce;
        private Vector2 _force;

        private void Awake()
        {
            CalculateForce(_windForce);
        }

        private void CalculateForce(float _windValue)
        {
            _force = (transform.right * _windValue);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log(_force);
                collision.gameObject.GetComponent<PlayerController>().ApplyWind(_force);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<PlayerController>().ApplyWind(_force);
            }
        }


    }
}


