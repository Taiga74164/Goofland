using Enemies;
using Levels;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Piano : MonoBehaviour, IWeapon
    {
        [SerializeField] private float fallSpeed = 10.0f;
        
        private Rigidbody2D _rigidbody2D;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                other.gameObject.GetComponent<Enemy>().GotHit(this);
            else if (other.gameObject.GetComponent<IBreakable>() != null)
                // Break the object.
                Destroy(gameObject);
            
            // Destroy the piano after it hits something.
            Destroy(gameObject);
        }

        public void DropPiano()
        { 
            _rigidbody2D.velocity = new Vector2(0.0f, -fallSpeed);
        }
    }
}