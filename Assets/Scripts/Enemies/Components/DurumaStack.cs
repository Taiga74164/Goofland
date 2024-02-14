using Controller;
using UnityEngine;

namespace Enemies.Components
{
    public class DurumaStack : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer())
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy: transform);
        }
    }
}