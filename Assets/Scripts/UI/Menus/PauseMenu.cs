using Managers;
using Objects;
using UnityEngine;
using Utils;

namespace UI.Menus
{
    public class PauseMenu : Menu
    {
        [SerializeField] private SettingsMenu settingsMenu;
        
        private void OnEnable() => GameManager.IsPaused = true;
        
        private void OnDisable() => GameManager.IsPaused = false;

        public void OnRestartButtonClicked() => LevelUtil.RestartLevel();

        public void OnPlayButtonClicked() => CloseMenu();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
