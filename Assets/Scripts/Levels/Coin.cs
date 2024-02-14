using Managers;
using UnityEngine;

namespace Levels
{
    public class Coin : MonoBehaviour
    {
        public CoinValue coinValue = CoinValue.D1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.IsPlayer())
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
