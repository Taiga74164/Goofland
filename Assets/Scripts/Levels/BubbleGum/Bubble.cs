using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Levels
{
    public class Bubble : MonoBehaviour
    {
        private CircleCollider2D circleCollider;//may use later to check the exact point of contact
        [SerializeField] private float _explosionForce;

        //deactivates at the start. Gum object will set this to active when hit by a pie
        private void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareLayer("Player"))
            {
                Vector2 force = -collision.relativeVelocity.normalized * _explosionForce;
                collision.gameObject.GetComponent<PlayerController>().Bounced(force);
                gameObject.SetActive(false);
            }
        }
    }
}

