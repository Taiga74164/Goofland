using Enemies.Components;
using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Enemies
{
    public class FartyCloud : Enemy
    {
        [Header("FartyCloud Settings")]
        [Tooltip("The duration of the exhale.")]
        [SerializeField] private float exhaleDuration = 3.0f;
        [Tooltip("The delay between exhales.")]
        [SerializeField] private float exhaleDelay = 3.0f;
        [Tooltip("The area of effect of the fart cloud.")]
        [SerializeField] private float areaOfEffect = 1.0f;

        private float _exhaleTimer;
        
        protected override void Update()
        {
            if (GameManager.IsPaused) return;
            
            // Turn the enemy if the player is in line of sight.
            if (!PlayerInLineOfSight()) return;
            Turn();
            
            // Handle the exhale of fart clouds.
            HandleFartCloud();
        }
        
        private void HandleFartCloud()
        {
            if (Time.time >= _exhaleTimer)
            {
                ExhaleFartCloud();
                _exhaleTimer = Time.time + exhaleDelay + exhaleDuration;
            }
        }
        
        private void ExhaleFartCloud()
        {
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Create a fart cloud.
            var fartCloud = PrefabManager.Create<FartCloudEffect>(Prefabs.FartCloudEffect, transform);
            fartCloud.fartArea = areaOfEffect;
            fartCloud.transform.position = transform.position.x > playerPosition.x
                ? transform.position + new Vector3(-1.0f, 0.0f, 0.0f)
                : transform.position + new Vector3(1.0f, 0.0f, 0.0f);
            
            // Destroy the fart cloud after the exhale duration.
            Destroy(fartCloud.gameObject, exhaleDuration);
        }

        protected override void MoveEnemy()
        {
        }

        protected override void Timer()
        {
        }
        
        protected override void Turn()
        {
            // Get the player's position.
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Determine the direction to face.
            var playerIsRight = playerPosition.x > transform.position.x;
            
            // Set the direction and model rotation.
            direction = playerIsRight ? Vector2.right : Vector2.left;
            model!.transform.eulerAngles = playerIsRight ? Vector3.zero : new Vector3(0, 180, 0);
        }
    }
}