using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public GameObject healthContainer;
        
        public TMP_Text timeText;
        private float _timer;
        
        private void Update()
        {
            HandleHealthChanged(GameManager.Instance.playerController.CurrentHealth);
        }
        
        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            _timer += Time.fixedDeltaTime;
            timeText.text = $"Time: {TimeSpan.FromSeconds(_timer):mm\\:ss}";
        }
        
        private void HandleHealthChanged(int health)
        {
            for (var i = 0; i < healthContainer.transform.childCount; i++)
            {
                var heart = healthContainer.transform.GetChild(i).gameObject;
                heart.SetActive(i < health);
            }
        }
    }
}
