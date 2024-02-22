using UnityEngine;

namespace UI
{
    public class ParallaxEffect : MonoBehaviour
    {
        [Tooltip("The direction of the parallax effect.")]
        public ParallaxDirection parallaxDirection = ParallaxDirection.Horizontal;
        public float parallaxEffectMultiplier = 0.5f;
        public float parallaxEffectMultiplierY = 0.5f;
        
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private float _textureUnitSizeX;
        private float _textureUnitSizeY;
        
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
            _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }
        
        private void LateUpdate()
        {
            // Calculate the distance the camera has moved since the last frame.
            var deltaMovement = _cameraTransform.position - _lastCameraPosition;

            switch (parallaxDirection)
            {
                case ParallaxDirection.Horizontal:
                    HorizontalParallax(deltaMovement);
                    break;
                case ParallaxDirection.Vertical:
                    VerticalParallax(deltaMovement);
                    break;
            }
        }
        
        private void HorizontalParallax(Vector3 deltaMovement)
        {
            // Move the background by the distance the camera has moved times the parallax effect multiplier.
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0);
            _lastCameraPosition = _cameraTransform.position;
            
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                var offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
        
        private void VerticalParallax(Vector3 deltaMovement)
        {
            // Move the background by the distance the camera has moved times the parallax effect multiplier.
            transform.position += new Vector3(0, deltaMovement.y * parallaxEffectMultiplierY);
            _lastCameraPosition = _cameraTransform.position;
            
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitSizeY)
            {
                var offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y + offsetPositionY);
            }
        }
    }
    
    public enum ParallaxDirection
    {
        Horizontal,
        Vertical
    }
}