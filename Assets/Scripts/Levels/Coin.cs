using Managers;
using UnityEngine;

namespace Levels
{
    public class Coin : MonoBehaviour
    {
        public CoinValue coinValue = CoinValue.D1;
        [SerializeField] private float despawnTime = 10.0f;
        
        private void Start() => Destroy(gameObject, despawnTime);
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer())
            {
                CurrencyManager.Instance.AddCurrency((int)coinValue);
                Destroy(gameObject);
            }
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
