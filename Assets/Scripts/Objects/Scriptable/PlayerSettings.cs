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
        public float movementSpeed = 5.0f;
        public float runSpeedMultiplier = 1.5f;
        public float crouchSpeedMultiplier = 0.5f;
        public float jumpHeight = 5.0f;
        public float knockbackForce;
        [Tooltip("Higher value, faster fall.")]
        public float fallMultiplier = 5.0f;
        [Tooltip("Lower value, shorter jump.")]
        public float lowJumpMultiplier = 2.0f;
    
        [Header("Coyote Time Settings")]
        public float coyoteTime = 0.2f;
        
        [Header("Jump Buffering Settings")]
        public float jumpBufferTime = 0.2f;
        public float jumpBoostMultiplier = 1.2f;
    }
}