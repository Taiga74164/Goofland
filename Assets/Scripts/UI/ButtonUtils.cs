using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ButtonUtils : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler// , IPointerUpHandler
    {
        #region Button Hover Effect

        private readonly Vector3 _originalScale = Vector3.one;
        private Vector3 _targetScale;
        private const float ScaleFactor = 1.2f;
        private const float LerpSpeed = 10.0f;

        #endregion
        
        public AudioSource audioSource;
        
        private void Start()
        {
            _targetScale = _originalScale;
            
            // This is to make sure that the button is not clickable when it is invisible.
            GetComponent<Image>().alphaHitTestMinimumThreshold = 1.0f;
        }
    
        private void Update()
        {
            if (_targetScale == transform.localScale)
                return;
        
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, LerpSpeed * Time.deltaTime);
        }
        
        public void OnPointerEnter(PointerEventData eventData) => _targetScale = _originalScale * ScaleFactor;
    
        public void OnPointerExit(PointerEventData eventData) => _targetScale = _originalScale;
        // public void OnPointerUp(PointerEventData eventData) => audioSource.Play();
    }
}
