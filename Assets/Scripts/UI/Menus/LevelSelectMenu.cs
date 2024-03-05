using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus
{
    public class LevelSelectMenu : Menu
    {
        [Header("Level Select Menu")]
        [SerializeField] private UDictionary<string, Sprite> sceneSprites;
        [SerializeField] private Vector2 transitionSize = new Vector2(1920, 2485);
        
        private string _levelName;

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.IsPaused = true;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.IsPaused = false;
        }


        public void SetLevelName(string levelName) => _levelName = levelName;
        
        public void OnEnterLevel()
        {
            var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            
            Sprite transitionSprite = null;
            if (sceneSprites.TryGetValue(_levelName, out var sprite))
                transitionSprite = sprite;
            
            TransitionManager.Instance.LoadScene(_levelName, TransitionManager.TransitionType.ZoomAndFade, transitionSprite,
                false, transitionSize);
        }
    }
}