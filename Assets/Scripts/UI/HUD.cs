using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public TMP_Text coinText;
        public TMP_Text timeText;
        private float _timer;

        private void Update()
        {
            coinText.text = $"{CurrencyManager.Instance.Currency}";
        }
        
        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            _timer += Time.fixedDeltaTime;
            timeText.text = $"Time: {TimeSpan.FromSeconds(_timer):mm\\:ss}";
        }
    }
}
