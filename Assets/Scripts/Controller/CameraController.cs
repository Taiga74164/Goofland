using Managers;
using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        public float offsetX = 5f;
        public float maxVerticalDistance = 10f;
        
        private void Update()
        {
            var playerPosition = GameManager.Instance.playerController.transform.position;
            var position = transform.position;
            var targetPosition = new Vector3(playerPosition.x + offsetX, position.y, position.z);
            var verticalDistance = Mathf.Abs(playerPosition.y - position.y);

            if (verticalDistance > maxVerticalDistance)
                targetPosition.y = playerPosition.y;

            transform.position = targetPosition;
        }
    }
}