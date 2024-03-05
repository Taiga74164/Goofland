using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus
{
    public class LevelSelectMenu : Menu
    {
        private string _levelName;
        
        public void SetLevelName(string levelName) => _levelName = levelName;
        
        public void OnEnterLevel()
        {
            var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            var image = button.GetComponent<Image>();
            TransitionManager.Instance.LoadScene(_levelName, TransitionManager.TransitionType.Zoom, image.sprite,
                false, new Vector2(1920, 2485));
        }
    }
}