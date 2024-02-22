using Enemies;
using Levels;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Piano : MonoBehaviour, IWeapon
    {
        [SerializeField] private float fallSpeed = 10.0f;
        public bool despawn;
        
        private Rigidbody2D _rigidbody2D;
        private EdgeCollider2D _edgeCollider2D;
        private BoxCollider2D _boxCollider2D;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;
            
            _edgeCollider2D = GetComponent<EdgeCollider2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            
            // If the piano is attached to the balloon, the top side of it can only be stepped on.
            _edgeCollider2D.enabled = true;
            _boxCollider2D.enabled = false;
        }

        private void Update()
        {
            if (!despawn && transform.position.y < -20f)
            {
                if (GetComponentInParent<Balloon>() != null)
                    GetComponentInParent<Balloon>().Respawn();
                
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy") && !despawn)
            {
                other.gameObject.GetComponent<Enemy>().GotHit(this);
                Destroy(gameObject);
            }
            else if (other.gameObject.GetComponent<IBreakable>() != null && !despawn)
            {
                Destroy(other.gameObject);
            }
            else if (other.IsPlayer())
            {
                other.transform.SetParent(transform);
            }
            else if (!despawn)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.IsPlayer())
                collision.transform.parent = null;
        }

        public void DropPiano()
        {
            // Reverse the colliders.
            _edgeCollider2D.enabled = false;
            _boxCollider2D.enabled = true;
            // Drop the piano.
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.velocity = new Vector2(0.0f, -fallSpeed);
        }
    }
}