using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public TMP_Text coinText;
        public TMP_Text defeatedText;
        public TMP_Text timeText;
        private float _timer;

        private void Update()
        {
            coinText.text = $"{CurrencyManager.Instance.Currency}";
            defeatedText.text = $"Defeated: {EntityManager.Instance.defeatedEnemies.Count}";
        }
        
        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            _timer += Time.fixedDeltaTime;
            timeText.text = $"Time: {TimeSpan.FromSeconds(_timer):mm\\:ss}";
        }
    }
}
