using UnityEngine;

public class WaterGunProjectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // TODO: Deal damage to the enemy.
        }
    }
}
