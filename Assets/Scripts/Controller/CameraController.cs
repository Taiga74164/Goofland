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
            var playerTransform = GameManager.Instance.playerController.transform;
            var targetPosition = new Vector3(playerTransform.position.x + offsetX, transform.position.y, transform.position.z);
            var verticalDistance = Mathf.Abs(playerTransform.position.y - transform.position.y);

            if (verticalDistance > maxVerticalDistance)
                targetPosition.y = playerTransform.position.y;

            transform.position = targetPosition;
        }
    }
}