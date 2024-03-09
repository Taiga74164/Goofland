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

        [Header("FartyCloud Audio Settings")]
        [SerializeField] private AudioData inhaleAudioData;
        [SerializeField] private AudioData exhaleAudioData;
        
        private float _exhaleCooldown;

        protected override void Start()
        {
            base.Start();
            
            // Set the exhale timer.
            _exhaleCooldown = exhaleDelay;
        }

        protected override void Update()
        {
            base.Update();
            
            if (GameManager.IsPaused) return;

            if (!PlayerInLineOfSight()) return;
            
            // Turn the enemy if the player is in line of sight.
            Turn();
            
            // Handle the exhale of fart clouds.
            HandleFartCloud();
        }
        
        private void HandleFartCloud()
        {
            if (entityType is not EntityType.Enemy) return;
            
            // Decrement the exhale timer.
            _exhaleCooldown -= Time.deltaTime;
            
            if (_exhaleCooldown <= 0)
            {
                // Exhale a fart cloud.
                ExhaleFartCloud();
                
                // Set the next exhale time.
                _exhaleCooldown = exhaleDelay + exhaleDuration;
            }
            else
            {
                if (audioSource.clip == inhaleAudioData.clip || !(_exhaleCooldown <= exhaleDuration)) return;
                
                // Play the inhale audio.
                audioSource.Configure(inhaleAudioData);
                audioSource.Play();
                
                // Set the inhale animation.
                animator.SetBool("Exhale", false);
                animator.SetBool("Inhale", true);
            }
        }
        
        private void ExhaleFartCloud()
        {
            // Play the exhale audio.
            audioSource.Configure(exhaleAudioData);
            audioSource.Play();
            
            // Set the exhale animation.
            animator.SetBool("Inhale", false);
            animator.SetBool("Exhale", true);
            
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

        protected override void Patrol()
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