using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class Bubble : MonoBehaviour
    {
        private CircleCollider2D circleCollider;//may use later to check the exact point of contact
        [SerializeField] private float _explosionForce;
        private void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Vector2 force = -collision.relativeVelocity.normalized * _explosionForce;
                collision.gameObject.GetComponent<Controller.PlayerController>().Bounced(force);
                gameObject.SetActive(false);
            }
        }
    }
}

