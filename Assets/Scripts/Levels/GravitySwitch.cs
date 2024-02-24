using UnityEngine;

namespace Levels
{
    public class GravitySwitch : MonoBehaviour
    {
        private void Awake()
        {
            Physics2D.gravity = new Vector2(0,-9.8f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareLayer("Projectile"))
            {
                Physics2D.gravity *= -1;
            }
        }
    }
}


