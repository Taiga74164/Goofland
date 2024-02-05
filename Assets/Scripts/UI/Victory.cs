using Managers;
using Objects;
using UnityEngine;

namespace UI
{
    public class Victory : Menu
    {
        [SerializeField] private SettingsMenu settingsMenu;
        
        private void OnEnable() => GameManager.IsPaused = true;
        
        public void OnHomeButtonClicked() => LevelManager.LoadLevel("MainMenu");
        
        public void OnRestartButtonClicked() => LevelManager.RestartLevel();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
