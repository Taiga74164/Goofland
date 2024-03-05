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
        private bool _isExhaling;
        private bool _canInhale = true;

        protected override void Start()
        {
            base.Start();
            
            // Set the exhale timer.
            _exhaleCooldown = exhaleDelay;
        }

        protected override void Update()
        {
            base.Update();

            if (PlayerInLineOfSight())
            {
                // Turn the enemy if the player is in line of sight.
                Turn();
            
                // Handle the exhale of fart clouds.
                HandleFartCloud();
            }
            else
            {
                _isExhaling = false;
                _canInhale = true;
            }
        }
        
        private void HandleFartCloud()
        {
            if (entityType is not EntityType.Enemy) return;
            
            // TODO: Fix SFX audio bug.
            
            // Decrement the exhale timer.
            _exhaleCooldown -= Time.deltaTime;
            
            if (_exhaleCooldown <= 0 && !_isExhaling)
            {
                _isExhaling = true;
                _canInhale = false;
                
                // Play the exhale audio.
                if (audioSource.isPlaying)
                    audioSource.Stop();
                audioSource.PlayOneShot(exhaleAudioData.clip);
                
                // Exhale a fart cloud.
                ExhaleFartCloud();
                
                // Set the next exhale time.
                _exhaleCooldown = exhaleDelay + exhaleDuration;
            }
            else if (_isExhaling)
            {
                _canInhale = false;
            }
            else
            {
                if (_canInhale)
                {
                    // Play the inhale audio once after exhaling is done.
                    if (audioSource.isPlaying)
                        audioSource.Stop();
                    audioSource.PlayOneShot(inhaleAudioData.clip);
                    _canInhale = false;
                }
            }
            
            if (_exhaleCooldown <= exhaleDelay)
            {
                _isExhaling = false;
                _canInhale = true;
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