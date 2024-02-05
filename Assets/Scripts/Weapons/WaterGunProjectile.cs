using UnityEngine;

namespace Weapons
{
    public class WaterGunProjectile : MonoBehaviour, IWeapon
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().GotHit(this);
            }
        }
    
        public bool Enabled
        {
            get => this.enabled;
            set => this.enabled = value;
        }
    }
}
