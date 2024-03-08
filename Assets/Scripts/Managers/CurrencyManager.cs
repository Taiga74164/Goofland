using System;
using System.Collections.Generic;
using Levels;
using Objects.Scriptable;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// A struct that represents a currency drop.
    /// </summary>
    [Serializable]
    public struct CurrencyDrop
    {
        public CoinValue coinValue;
        public int quantity;
        
        public CurrencyDrop(CoinValue coinValue, int quantity)
        {
            this.coinValue = coinValue;
            this.quantity = quantity;
        }
    }
    
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public int Currency { get; private set; }

        private AudioData _dropAudioData;

        private void Start()
        {
            _dropAudioData = Resources.Load<AudioData>("AudioData/SFX/Dice_Roll");
            if (!_dropAudioData) Debug.LogError("AudioData/SFX/Dice_Roll not found!");
        }
        
        /// <summary>
        /// Calculates the number of dice drops for a given score.
        /// </summary>
        /// <param name="score">The score to calculate.</param>
        /// <returns>A dictionary of dice drops.</returns>
        public static Dictionary<CoinValue, int> CalculateDiceDrops(int score)
        {
            var drops = new Dictionary<CoinValue, int>();
            var diceOrder = (CoinValue[])Enum.GetValues(typeof(CoinValue));
            // Sort the dice order in descending order (highest to lowest).
            Array.Sort(diceOrder, (a, b) => b.CompareTo(a));
            
            foreach (var dice in diceOrder)
            {
                // Get the value of the dice.
                var diceValue = (int)dice;
                // Calculate the number of dice drops.
                var count = score / diceValue;
                
                if (count > 0)
                {
                    // Add the dice to the dictionary.
                    drops[dice] = count;
                    // Subtract the score from the total.
                    score -= count * diceValue;
                }
            }

            return drops;
        }

        /// <summary>
        /// Drops currency at a given position.
        /// </summary>
        /// <param name="coinValue">The value of the currency to drop.</param>
        /// <param name="quantity">The quantity of currency to drop.</param>
        /// <param name="dropForce">The force to apply to the currency when dropped.</param>
        /// <param name="offset">The offset distance behind the player.</param>
        /// <param name="position">The position to drop the currency.</param>
        /// <param name="direction">The direction to drop the currency.</param>
        public static void DropCurrency(CoinValue coinValue, int quantity,
            float dropForce = 5.0f, float offset = 1.0f, Vector3 position = default, Vector3 direction = default)
        {
            for (var i = 0; i < quantity; i++)
            {
                // Create the currency prefab.
                var obj = PrefabManager.Create(coinValue.ToCurrencyPrefab());
                
                // Set the position of the currency.
                var spawnPosition = position + (-direction.normalized * offset) + Vector3.up;
                obj.transform.position = spawnPosition;
                
                // Apply a force to the currency.
                var forceDirection = new Vector2(-direction.x, 2).normalized * dropForce;
                
                var rbCoin = obj.GetComponent<Rigidbody2D>();
                rbCoin.AddForce(forceDirection, ForceMode2D.Impulse);
                
                // Delay the collection of the currency.
                var coin = obj.GetComponent<Coin>();
                coin!.DelayMagnetization();
            }
            
            // Play the currency drop sound.
            AudioManager.Instance.PlayOneShotAudio(Instance._dropAudioData, position);
        }
        
        public void AddCurrency(int amount) => Currency += amount;

        public void RemoveCurrency(int amount) => Currency -= Mathf.Clamp(amount, 0, Currency);

        public void RemoveCurrency(float percentage) => 
            RemoveCurrency(Mathf.RoundToInt(Currency * (percentage / 100.0f)));
        
        public void ResetCurrency() => Currency = 0;
    }
}