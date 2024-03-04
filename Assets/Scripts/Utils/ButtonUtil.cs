using Managers;
using Objects.Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils
{
    /// <summary>
    /// Provides utility functions for buttons.
    /// </summary>
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ButtonUtil : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        #region Button Hover Effect

        private readonly Vector3 _originalScale = Vector3.one;
        private Vector3 _targetScale;
        private const float ScaleFactor = 1.2f;
        private const float LerpSpeed = 10.0f;

        #endregion
        
        #region Audio Feedback
        
        private AudioData _mouseClickData;
        private AudioData _mouseSelectData;
        
        #endregion
        
        private void Awake()
        {
            // Load the audio data from the Resources folder.
            _mouseClickData = Resources.Load<AudioData>("SoundData/Mouse_Click");
            _mouseSelectData = Resources.Load<AudioData>("SoundData/Mouse_Hover");
        }
        
        private void Start()
        {
            _targetScale = _originalScale;
            
            // This is to make sure that the button is not clickable when it is invisible.
            GetComponent<Image>().alphaHitTestMinimumThreshold = 1.0f;
            
            // Play the audio when the button is clicked.
            GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.PlayAudio(_mouseClickData));
        }
    
        private void Update()
        {
            if (_targetScale == transform.localScale)
                return;
        
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, LerpSpeed * Time.deltaTime);
        }
        
        public void OnPointerEnter(PointerEventData eventData) => OnButtonEnter(ScaleFactor);
    
        public void OnPointerExit(PointerEventData eventData) => OnButtonExit();
        
        public void OnSelect(BaseEventData eventData) => OnButtonEnter(ScaleFactor);

        public void OnDeselect(BaseEventData eventData) => OnButtonExit();
        
        private void OnButtonEnter(float scaleFactor)
        {
            _targetScale = _originalScale * scaleFactor;
            AudioManager.Instance.PlayAudio(_mouseSelectData);
        }
        
        private void OnButtonExit() => _targetScale = _originalScale;
    }
}
