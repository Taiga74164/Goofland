using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Scriptable
{
    /// <summary>
    /// Represents player settings for use in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "Player", menuName = "Player/Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Player Settings")]
        public float movementSpeed = 6.0f;
        public float runSpeedMultiplier = 1.8f;
        public float crouchSpeedMultiplier = 0.5f;
        public float jumpHeight = 8.0f;
        public float jumpBoostMultiplier = 1.2f;
        public float horizontalKnockback;
        public float verticalKnockback;
        [Tooltip("Higher value, faster fall.")]
        public float fallMultiplier = 5.0f;
        [Tooltip("Lower value, shorter jump.")]
        public float lowJumpMultiplier = 4.0f;
        public float maxVelocity = 20f;

        [Header("Umbrella Setting")]
        public float umbrellaGravityDiminisher = 0.25f;
        [Tooltip("Amount of time player can use the umbrella.")]
        public float umbrellaTime = 2.5f;
    
        [Header("Coyote Time Settings")]
        public float coyoteTime = 0.2f;
        
        [Header("Jump Buffering Settings")]
        public float jumpBufferTime = 0.2f;
        
        [Header("Player Health Settings")]
        public int maxHealth = 3;
        
        [Header("Ground Check Settings")]
        public float groundCheckRadius = 0.1f;
        [Tooltip("The layer mask for the ground.")]
        public LayerMask groundLayerMask;
        
        [Header("Invincibility Settings")]
        public float invincibilityDuration = 2.0f;
        
        [Header("Magnet Settings")]
        [Tooltip("The radius of the magnet.")]
        public float magnetRadius = 1.0f;
        [Tooltip("The force of the magnet.")]
        public float magnetForce = 5.0f;
        [Tooltip("The layer mask of the magnet.")]
        public LayerMask magnetLayer;
        
        [Header("Currency Drop Settings")]
        public float dropForce = 5.0f;
        public float dropOffset = 1.0f;
        
        [Header("Audio Data")]
        public AudioData jumpAudioData;
        public AudioData runAudioData;
        public AudioData walkAudioData;
        public AudioData umbrellaAudioData;
        public AudioData fartAudioData;
        public AudioData attackAudioData;
    }
}