using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Managers
{
    public class TransitionManager : Singleton<TransitionManager>
    {
        [Header("Transition Settings")]
        [SerializeField] [CanBeNull] private GameObject transitionPrefab;
        [SerializeField] [CanBeNull] private Material transitionMaterial;
        [SerializeField] [CanBeNull] private GameObject fadeOverlayPrefab;
        [SerializeField] private float transitionDuration = 1.0f;
        
        private Image _transitionImage;
        private Image _fadeOverlay;
        private static readonly int MaskTex = Shader.PropertyToID("_MaskTex");
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        
        /// <summary>
        /// Special singleton initializer method.
        /// </summary>
        public new static void Initialize()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Managers/TransitionManager");
            if (prefab == null) throw new Exception("Missing TransitionManager prefab!");

            var instance = Instantiate(prefab);
            if (instance == null) throw new Exception("Failed to instantiate TransitionManager prefab!");

            instance.name = "Managers.TransitionManager (Singleton)";
        }
        
        protected override void OnAwake()
        {
            // Create the Canvas
            var canvasObj = new GameObject("TransitionCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // Set the sorting order to be on top of everything.
            canvas.sortingOrder = 1000;
            // Make the canvas persistent between scenes.
            DontDestroyOnLoad(canvasObj); 
            
            // Add CanvasScaler for UI scaling.
            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Add GraphicRaycaster for UI raycasting.
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // Create an Image if transitionPrefab is not set.
            if (transitionPrefab == null)
            {
                // Create the transition Image
                var imageObj = new GameObject("TransitionImage");
                imageObj.transform.SetParent(canvas.transform, false);
                imageObj.transform.localPosition = Vector3.zero;
                imageObj.transform.localScale = Vector3.one;
            
                _transitionImage = imageObj.AddComponent<Image>();
                _transitionImage.color = Color.white;
                _transitionImage.material = transitionMaterial;
                _transitionImage.enabled = false;
            }
            else
            {
                var imageObj = Instantiate(transitionPrefab, canvas.transform);
                imageObj.name = "TransitionImage";
                _transitionImage = imageObj.GetComponent<Image>();
                _transitionImage.material = transitionMaterial;
                _transitionImage.enabled = false;
            }
            
            // Create the fade overlay Image
            if (fadeOverlayPrefab == null)
            {
                var fadeObj = new GameObject("FadeOverlay");
                fadeObj.transform.SetParent(canvas.transform, false);
                fadeObj.transform.localPosition = Vector3.zero;
                fadeObj.transform.localScale = Vector3.one;
                    
                _fadeOverlay = fadeObj.AddComponent<Image>();
                _fadeOverlay.color = Color.black;
                _fadeOverlay.enabled = false;
            }
            else
            {
                var fadeObj = Instantiate(fadeOverlayPrefab, canvas.transform);
                fadeObj.name = "FadeOverlay";
                _fadeOverlay = fadeObj.GetComponent<Image>();
                _fadeOverlay.enabled = false;
            }
        }
        
        public void LoadScene(string sceneName, TransitionType transitionType = TransitionType.None) 
            => StartCoroutine(TransitionToScene(sceneName, transitionType));
        
        public void LoadScene(string sceneName, TransitionType transitionType, Sprite transitionSprite, 
            bool fullScreen = true, Vector2 size = default, Texture2D mask = null)
        {
            if (fullScreen)
            {
                // Set the transition Image to full screen.
                _transitionImage.rectTransform.anchorMin = Vector2.zero;
                _transitionImage.rectTransform.anchorMax = Vector2.one;
                _transitionImage.rectTransform.sizeDelta = Vector2.zero;
                // Set the fade overlay to full screen.
                _fadeOverlay.rectTransform.anchorMin = Vector2.zero;
                _fadeOverlay.rectTransform.anchorMax = Vector2.one;
                _fadeOverlay.rectTransform.sizeDelta = Vector2.zero;
            }
            else
            {
                // Set the transition Image to the specified size.
                _transitionImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                _transitionImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _transitionImage.rectTransform.sizeDelta = size == default ? new Vector2(1920, 1080) : size;
                _transitionImage.rectTransform.anchoredPosition = Vector2.zero;
                // Set the fade overlay to the specified size.
                _fadeOverlay.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                _fadeOverlay.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _fadeOverlay.rectTransform.sizeDelta = size == default ? new Vector2(1920, 1080) : size;
                _fadeOverlay.rectTransform.anchoredPosition = Vector2.zero;
            }

            _transitionImage.material.SetTexture(MaskTex, mask != null ? mask : null);

            _transitionImage.sprite = transitionSprite;
            StartCoroutine(TransitionToScene(sceneName, transitionType));
        }
        
        private IEnumerator TransitionToScene(string sceneName, TransitionType transitionType)
        {
            // Start Transition.
            yield return StartCoroutine(PerformTransition(transitionType, true));
            // Load Scene.
            LevelUtil.LoadLevel(sceneName);
            // End Transition.
            yield return StartCoroutine(PerformTransition(transitionType, false));
        }
        
        private IEnumerator PerformTransition(TransitionType transitionType, bool isEntering)
        {
            if (_transitionImage)
                _transitionImage.enabled = true;
            
            switch (transitionType)
            {
                case TransitionType.None:
                    break;
                case TransitionType.Fade:
                    yield return StartCoroutine(FadeTransition(isEntering));
                    break;
                case TransitionType.HorizontalSlide:
                    yield return StartCoroutine(HorizontalSlideTransition(isEntering));
                    break;
                case TransitionType.VerticalSlide:
                    yield return StartCoroutine(VerticalSlideTransition(isEntering));
                    break;
                case TransitionType.Zoom:
                    yield return StartCoroutine(ZoomTransition(isEntering));
                    break;
                case TransitionType.Dissolve:
                    yield return StartCoroutine(DissolveTransition(isEntering));
                    break;
                case TransitionType.ZoomAndFade:
                    yield return StartCoroutine(ZoomAndFadeTransition(isEntering));
                    break;
            }
            
            // Set Transition Image to Hidden.
            if (!isEntering && _transitionImage)
                _transitionImage.enabled = false;
        }
        
        private IEnumerator FadeTransition(bool isEntering, Color fadeColor = default)
        {
            var material = _transitionImage.material;
            
            var start = isEntering ? 1.0f : 0.0f;
            var end = isEntering ? 0.0f : 1.0f;
            var t = 0.0f;
            
            _transitionImage.enabled = true;
            
            while (t <= transitionDuration)
            {
                t += Time.deltaTime;
                
                // // Set Alpha.
                // material.SetFloat("_Cutoff", Mathf.Lerp(start, end, t));
                // // Set Color.
                // material.SetColor("_FadeColor",
                //     Color.Lerp(fadeColor == default ? Color.white : fadeColor, Color.clear, t));
                
                var alpha = Mathf.Lerp(start, end, t);
                _transitionImage.color = new Color(0, 0, 0, alpha);
                
                yield return null;
            }
            
            // Reset Alpha.
            _transitionImage.color = new Color(0, 0, 0, end);
        }
        
        private IEnumerator HorizontalSlideTransition(bool isEntering)
        {
            var start = isEntering ? 1.0f : 0.0f;
            var end = isEntering ? 0.0f : 1.0f;
            var t = 0.0f;
            
            while (t <= transitionDuration)
            {
                t += Time.deltaTime;
                
                var slide = Mathf.Lerp(start, end, t);
                _transitionImage.rectTransform.anchorMax = new Vector2(slide, 1.0f);
                
                yield return null;
            }
        }
        
        private IEnumerator VerticalSlideTransition(bool isEntering)
        {
            var start = isEntering ? 1.0f : 0.0f;
            var end = isEntering ? 0.0f : 1.0f;
            var t = 0.0f;
            
            while (t <= transitionDuration)
            {
                t += Time.deltaTime;
                
                // Set Anchor Max.
                var slide = Mathf.Lerp(start, end, t);
                _transitionImage.rectTransform.anchorMax = new Vector2(1.0f, slide);
                
                yield return null;
            }
        }
        
        private IEnumerator ZoomTransition(bool isEntering)
        {
            var startScale = isEntering ? Vector3.zero : Vector3.one;
            var endScale = isEntering ? Vector3.one : Vector3.zero;
            var t = 0.0f;
            
            _transitionImage.enabled = true;
            
            while (t <= transitionDuration)
            {
                t += Time.deltaTime;
                
                // Set Scale.
                _transitionImage.rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }
            
            // Reset Scale.
            _transitionImage.rectTransform.localScale = endScale;
        }
        
        private IEnumerator DissolveTransition(bool isEntering)
        {
            var material = _transitionImage.material;
            var start = isEntering ? 1.0f : 0.0f;
            var end = isEntering ? 0.0f : 1.0f;
            var t = 0.0f;
            
            while (t <= transitionDuration)
            {
                t += Time.deltaTime;
                
                // Set Alpha.
                // transitionImage.material.SetFloat("_Cutoff", Mathf.Lerp(start, end, t));
                // Set Dissolve Amount.
                material.SetFloat(DissolveAmount, Mathf.Lerp(start, end, t));
                
                yield return null;
            }
            
            // Reset Dissolve Amount.
            material.SetFloat(DissolveAmount, end);
        }
        
        private IEnumerator ZoomAndFadeTransition(bool isEntering)
        {
            // Zoom Transition.
            var startScale = isEntering ? Vector3.zero : Vector3.one;
            var endScale = isEntering ? Vector3.one : Vector3.zero;
            // Fade Transition.
            var startColor = isEntering ? new Color(0, 0, 0, 0) : new Color(0, 0, 0, 1);
            var endColor = isEntering ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0);
            
            var t = 0.0f;
            _transitionImage.enabled = true;
            _fadeOverlay.enabled = true;
            
            while (t <= transitionDuration)
            {
                t += Time.deltaTime;
                
                // Set Scale.
                _transitionImage.rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                // Set Fade Color.
                _fadeOverlay.color = Color.Lerp(startColor, endColor, t);
                
                yield return null;
            }
            
            // Reset Scale and Fade Color.
            _transitionImage.rectTransform.localScale = endScale;
            _fadeOverlay.color = endColor;
        }
        
        public enum TransitionType
        {
            None,
            Fade,
            HorizontalSlide,
            VerticalSlide,
            Zoom,
            Dissolve,
            ZoomAndFade,
        }
    }
}