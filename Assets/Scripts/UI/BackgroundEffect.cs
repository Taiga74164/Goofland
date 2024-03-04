using UnityEngine;

namespace UI
{
    public class BackgroundEffect : MonoBehaviour
    {
        [SerializeField] private BackgroundMode mode = BackgroundMode.Parallax;
        
        [SerializeField] private float parallaxEffectMultiplier = 0.5f;
        [SerializeField] private ParallaxDirection parallaxDirection = ParallaxDirection.Left;
        
        [SerializeField] private float panningSpeed = 1.5f;
        [SerializeField] private PanningDirection panningDirection = PanningDirection.Left;
        
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
            switch (mode)
            {
                case BackgroundMode.Parallax:
                    Parallax();
                    break;
                case BackgroundMode.Panning:
                    Panning();
                    break;
                default:
                    Parallax();
                    break;
            }
        }
        
        private void Parallax()
        {
            // Calculate the distance the camera has moved since the last frame.
            var deltaMovement = _cameraTransform.position - _lastCameraPosition;
            // Move the background by the distance the camera has moved times the parallax effect multiplier.
            transform.position += new Vector3(parallaxDirection == ParallaxDirection.Left ? -1 : 1, 0, 0) * 
                                  (deltaMovement.x * parallaxEffectMultiplier);
            RepeatBackground();
            _lastCameraPosition = _cameraTransform.position;
        }
        
        private void Panning()
        {
            transform.Translate((panningDirection == PanningDirection.Left ? Vector3.left : Vector3.right) * 
                                (panningSpeed * Time.deltaTime));
            RepeatBackground();
        }
        
        private void RepeatBackground()
        {
            var deltaX = _cameraTransform.position.x - transform.position.x;
            if (deltaX >= _textureUnitSizeX)
            {
                var offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
        
        public enum BackgroundMode
        {
            Parallax,
            Panning
        }
        
        public enum ParallaxDirection
        {
            Left,
            Right
        }
        
        public enum PanningDirection
        {
            Left,
            Right
        }
    }
}