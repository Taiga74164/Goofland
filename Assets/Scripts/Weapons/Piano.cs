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

        private void Update()
        {
            if(transform.position.y < -20f)
            {
                if(GetComponentInParent<Balloon>() != null)
                    GetComponentInParent<Balloon>().Respawn();
                
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().GotHit(this);
                Destroy(gameObject);
            }
                
            else if (other.gameObject.GetComponent<IBreakable>() != null)
                Destroy(other.gameObject);

            else if (other.IsPlayer())
                other.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.IsPlayer())
                collision.transform.parent = null;
        }

        public void DropPiano()
        { 
            _rigidbody2D.velocity = new Vector2(0.0f, -fallSpeed);
        }
    }
}