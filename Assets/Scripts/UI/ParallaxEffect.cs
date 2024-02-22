using UnityEngine;

namespace UI
{
    public class ParallaxEffect : MonoBehaviour
    {
        public float parallaxEffectMultiplier = 0.5f;
        
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private float _textureUnitSizeX;
        
        private void Start()
        {
            // Get the camera transform and the last camera position.
            _cameraTransform = Camera.main!.transform;
            _lastCameraPosition = _cameraTransform.position;
            
            // Get the sprite and the texture.
            var sprite = GetComponent<SpriteRenderer>().sprite;
            var texture = sprite.texture;
            
            // Get the texture unit size.
            _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }
        
        private void LateUpdate()
        {
            // Calculate the distance the camera has moved since the last frame.
            var deltaX = _cameraTransform.position.x - _lastCameraPosition.x;
            transform.position += new Vector3(deltaX * parallaxEffectMultiplier, 0, 0);
            _lastCameraPosition = _cameraTransform.position;

            // If the camera has moved far enough to the right, move the background to the right.
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                var offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
    }
}