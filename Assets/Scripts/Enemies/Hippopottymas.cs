using System.Collections;
using Managers;
using Objects.Scriptable;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public class Hippopottymas: Enemy
    {
        [Header("Hippopottymas Settings")]
        [SerializeField] private float chargeSpeed = 10.0f;
        [SerializeField] private GameObject disturbedModel;
        [SerializeField] private float disturbedDistance = 1.0f;
        public GameEvent onDisturbedEvent;
        
        [Header("Hippopottymas Audio Settings")]
        [SerializeField] private AudioData runAudioData;
        [SerializeField] private AudioData screamAudioData;
        [SerializeField] private AudioData flushAudioData;
        
        private bool _isDisturbed;
        private bool _canCharge;
        private bool _delayedCharge;
        private Vector3 _initialPosition;
        
        protected override void Start()
        {
            base.Start();
            
            _initialPosition = transform.position;
        }
        
        protected override void Update()
        {
            base.Update();
            
            if (entityType is not EntityType.Enemy) return;
            
            // Check if the player is in the disturbed range and not already disturbed, raise the event.
            if (IsInDisturbedRange() && !_isDisturbed)
            {
                onDisturbedEvent.Raise(gameObject);
                _isDisturbed = true;
            }
            else if (!PlayerInLineOfSight())
            {
                ReturnToInitialPosition();
            }
            
            if (!_isDisturbed) return;
            UpdateChargeCondition();
            Charge();
        }
        
        private void UpdateChargeCondition()
        {
            // Calculate direction to player.
            var playerPosition = playerTransform.position;
            var directionToPlayer = (playerPosition - transform.position).normalized;
    
            // Check if the player is to the left or right of the enemy.
            var isPlayerLeft = directionToPlayer.x < 0;
            var checkDirection = isPlayerLeft ? Vector2.left : Vector2.right;

            // Perform ground and wall checks in the direction towards the player.
            var groundPosition = groundDetection.position;
            var groundForward = groundPosition.Add(checkDirection * rayLength);
            var down = new Vector2(0, -0.5f);
            var groundInfo = Physics2D.Raycast(groundForward, down, rayLength, groundLayer);
            var wallInfo = Physics2D.Raycast(groundPosition, checkDirection, rayLength, turnLayer);

            Debug.DrawLine(groundForward, groundForward.Add(down * rayLength), Color.yellow);
            Debug.DrawLine(groundPosition, groundPosition.Add(checkDirection * rayLength), Color.red);
            
            // Update charge condition based on ground and wall checks.
            _canCharge = groundInfo.collider && !wallInfo.collider && PlayerInLineOfSight();
        }
        
        public void OnDisturbed(object data)
        {
            if (data is not GameObject obj) return;
            if (obj != gameObject) return;
            
            // Update the damage percentage when disturbed.
            damagePercentage = 20.0f;
            
            // Play the disturbed audio.
            AudioManager.Instance.PlayOneShotAudio(flushAudioData, transform.position);
            TimerManager.Instance.StartTimer(flushAudioData.clip.length, () =>
            {
                // Destroy the current model and replace it with the disturbed model.
                Destroy(model);
                model = disturbedModel;
                disturbedModel.SetActive(true);
            });
            
            // Start the delayed charge.
            StartCoroutine(DelayedCharge());
        }
        
        private IEnumerator DelayedCharge()
        {
            _delayedCharge = true;
            yield return new WaitForSeconds(flushAudioData.clip.length);
            _delayedCharge = false;
            AudioManager.Instance.PlayOneShotAudio(screamAudioData, transform.position);
        }

        private void Charge()
        {
            if (!_canCharge || _delayedCharge)
            {
                // Stop Running Audio
                if (audioSource.isPlaying && audioSource.clip == runAudioData.clip) audioSource.Stop();
                return;
            }
            
            // Get player position.
            var playerPosition = playerTransform.position;
            // Get direction towards player.
            var directionToPlayer = (playerPosition - transform.position).normalized;
            directionToPlayer.y = 0;
            
            // Move towards the player.
            transform.Translate(directionToPlayer * (chargeSpeed * Time.deltaTime));
            // Flip the model based on the direction to the player.
            Flip(directionToPlayer.x);
                
            // Play the run audio.
            if (audioSource.isPlaying) return;
            audioSource.Configure(runAudioData);
            audioSource.Play();
        }
        
        private void Flip(float directionX)
        {
            model!.transform.eulerAngles = directionX switch
            {
                < 0 => new Vector3(0, 180, 0),
                > 0 => Vector3.zero,
                _ => model!.transform.eulerAngles
            };
        }
        
        private bool IsInDisturbedRange()
        {
            if (entityType is not EntityType.Enemy) return false;
            
            // Get the enemy's position.
            var position = transform.position;
            var playerDirection  = playerTransform.position - position;
            // Cast a ray to check if the player is in the enemy's line of sight.
            var hit = Physics2D.Raycast(position, playerDirection, disturbedDistance, playerLayer);
            
            return hit.collider != null && hit.collider.IsPlayer();
        }

        private void ReturnToInitialPosition()
        {
            if (Vector3.Distance(transform.position, _initialPosition) < 0.1f) return;
            
            var directionToInitialPosition = (_initialPosition - transform.position).normalized;
            directionToInitialPosition.y = 0;
            transform.Translate(directionToInitialPosition * (chargeSpeed * Time.deltaTime));
            Flip(directionToInitialPosition.x);
        }
        
        protected override void Patrol()
        {
        }
        
        public override void GotHit(IWeapon weapon)
        {
            if (entityType is not EntityType.Enemy) return;
            
            switch (weapon)
            {
                case Pie when !_isDisturbed:
                    onDisturbedEvent.Raise(gameObject);
                    _isDisturbed = true;
                    break;
                case Piano:
                    OnHit();
                    StopAllCoroutines();
                    audioSource.Stop();
                    break;
            }
        }
    }
}