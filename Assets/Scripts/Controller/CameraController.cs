using Managers;
using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        [Header("Follow Settings")]
        public float offsetX = 5.0f;
        public float maxVerticalDistance = 10.0f;

        [Header("Smooth Follow Settings")]
        public bool enable;
        public float smoothTime = 0.2f;
        
        private Vector3 _velocity = Vector3.zero;
        
        private void LateUpdate()
        {
            var playerPosition = GameManager.Instance.playerController.transform.position;
            var position = transform.position;
            var targetPosition = new Vector3(playerPosition.x + offsetX, position.y, position.z);
            var verticalDistance = Mathf.Abs(playerPosition.y - position.y);

            // If the vertical distance is greater than the max vertical distance,
            // set the target position to the player's position.
            if (verticalDistance > maxVerticalDistance)
                targetPosition.y = playerPosition.y;

            transform.position = enable
                ? Vector3.SmoothDamp(position, targetPosition, ref _velocity, smoothTime)
                : targetPosition;
        }
    }
}