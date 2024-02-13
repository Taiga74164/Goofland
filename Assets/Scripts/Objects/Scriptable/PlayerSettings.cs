using UnityEngine;

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
        public float airborneMovementSpeed = 3.0f;
        public float knockbackForce;
        [Tooltip("Higher value, faster fall.")]
        public float fallMultiplier = 5.0f;
        [Tooltip("Lower value, shorter jump.")]
        public float lowJumpMultiplier = 4.0f;

        [Header("Umbrella Setting")]
        public float umbrellaGravityDiminisher = 0.25f;
        [Tooltip("amount of time player can use the umbrella")]
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
        
        [Header("Audio Data")]
        public AudioData jumpSoundData;
        public AudioData runSoundData;
        public AudioData walkSoundData;
        public AudioData fartSoundData;
    }
}