using Managers;
using Objects.Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Provides utility functions for buttons.
    /// </summary>
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ButtonUtils : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        
        private void Start()
        {
            _targetScale = _originalScale;
            
            // This is to make sure that the button is not clickable when it is invisible.
            GetComponent<Image>().alphaHitTestMinimumThreshold = 1.0f;
            
            // Load the audio data from the Resources folder.
            _mouseClickData = Resources.Load<AudioData>("SoundData/Mouse_Click");
            _mouseSelectData = Resources.Load<AudioData>("SoundData/Selection_Sound");
            
            // Play the audio when the button is clicked.
            GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.PlayAudio(_mouseClickData));
        }
    
        private void Update()
        {
            if (_targetScale == transform.localScale)
                return;
        
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, LerpSpeed * Time.deltaTime);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _targetScale = _originalScale * ScaleFactor;
            AudioManager.Instance.PlayAudio(_mouseSelectData);
        }
    
        public void OnPointerExit(PointerEventData eventData) => _targetScale = _originalScale;
    }
}
