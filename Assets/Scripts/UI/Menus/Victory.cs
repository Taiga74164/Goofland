using Managers;
using Objects;
using UnityEngine;
using Utils;

namespace UI.Menus
{
    public class Victory : Menu
    {
        [SerializeField] private SettingsMenu settingsMenu;
        
        private void OnEnable() => GameManager.IsPaused = true;
        
        public void OnHomeButtonClicked() => LevelUtil.LoadLevel("MainMenu");
        
        public void OnRestartButtonClicked() => LevelUtil.RestartLevel();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
