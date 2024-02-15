using System;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using Random = UnityEngine.Random;

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

        /// <summary>
        /// Calculates the number of dice drops for a given score.
        /// </summary>
        /// <param name="score">The score to calculate.</param>
        /// <returns>A dictionary of dice drops.</returns>
        public Dictionary<CoinValue, int> CalculateDiceDrops(int score)
        {
            var drops = new Dictionary<CoinValue, int>();
            var diceOrder = (CoinValue[])Enum.GetValues(typeof(CoinValue));

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
        /// <param name="scatterRadius">The radius to scatter the currency within.</param>
        /// <param name="position">The position to drop the currency.</param>
        public void DropCurrency(CoinValue coinValue, int quantity,
            float dropForce = 5.0f, float scatterRadius = 1.0f, Vector3 position = default)
        {
            for (var i = 0; i < quantity; i++)
            {
                // Create the currency prefab.
                var obj = PrefabManager.Create(coinValue.ToCurrencyPrefab());
                    
                // Scatter within a radius.
                var scatterPosition = Random.insideUnitSphere * scatterRadius;
                scatterPosition.y = 0;
                // Set the position to the enemy's position plus a random scatter position.
                obj.transform.position = position + scatterPosition;
                
                // Add a random force to the currency.
                var rbCoin = obj.GetComponent<Rigidbody2D>();
                var forceDirection = (Random.insideUnitSphere + Vector3.up).normalized;
                rbCoin.AddForce(forceDirection * dropForce, ForceMode2D.Impulse);
            }
        }
        
        public void AddCurrency(int amount) => Currency += amount;

        public void RemoveCurrency(int amount) => Currency -= Mathf.Clamp(amount, 0, Currency);

        public void RemoveCurrency(float percentage) => 
            RemoveCurrency(Mathf.RoundToInt(Currency * (percentage / 100)));
        
        public void ResetCurrency() => Currency = 0;
    }
}