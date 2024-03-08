using Controllers;
using UnityEngine;

namespace Levels
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private float explosionForce;
        
        private CircleCollider2D _circleCollider;//may use later to check the exact point of contact
        

        //deactivates at the start. Gum object will set this to active when hit by a pie
        private void Start()
        {
            _circleCollider = GetComponent<CircleCollider2D>();
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.IsPlayer())
            {
                Vector2 force = -collision.relativeVelocity.normalized * explosionForce;
                collision.gameObject.GetComponent<PlayerController>().Bounce(force);
                gameObject.SetActive(false);
            }
        }
    }
}

