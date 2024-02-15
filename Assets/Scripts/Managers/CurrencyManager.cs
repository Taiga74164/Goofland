using UnityEngine;

namespace Managers
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public int Currency { get; private set; }

        public void AddCurrency(int amount) => Currency += amount;

        public void RemoveCurrency(int amount) => Currency -= Mathf.Clamp(amount, 0, Currency);
        
        public void ResetCurrency() => Currency = 0;
    }
}