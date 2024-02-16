using Managers;
using UnityEngine;

namespace Levels
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private CoinValue coinValue = CoinValue.D1;
        [SerializeField] private float despawnTime = 10.0f;
        [SerializeField] private float collectionDelay = 1.0f;

        public bool CanMagnetize { get; private set; } = true;
        
        private void Start() => Destroy(gameObject, despawnTime);

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer() || other.gameObject.CompareTag("Projectile"))
                CollectCoin();
        }

        /// <summary>
        /// Collects the coin and adds its value to the currency manager.
        /// </summary>
        public void CollectCoin()
        {
            CurrencyManager.Instance.AddCurrency((int)coinValue);
            Destroy(gameObject);
        }

        public void DelayMagnetization()
        {
            CanMagnetize = false;
            TimerManager.Instance.StartTimer(collectionDelay, () => { CanMagnetize = true; });
        }
    }
    
    public enum CoinValue
    {
        D1 = 1,
        D4 = 4,
        D6 = 6, 
        D8 = 8,
        D10 = 10,
        D12 = 12,
        D20 = 20
    }
}
