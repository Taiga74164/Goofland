using Managers;
using UnityEngine;

namespace UI
{
    public class PauseMenu : Menu
    {
        [SerializeField] private SettingsMenu settingsMenu;
        
        private void OnEnable() => GameManager.Instance.IsPaused = true;
        
        private void OnDisable() => GameManager.Instance.IsPaused = false;
        
        public void OnRestartButtonClicked() => LevelManager.RestartLevel();

        public void OnPlayButtonClicked() => CloseMenu();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
