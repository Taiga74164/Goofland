using Managers;
using UnityEngine;

namespace UI
{
    public class Victory : MonoBehaviour
    {
        [SerializeField] private SettingsMenu settingsMenu;
        
        public void OnHomeButtonClicked() => LevelManager.LoadLevel("MainMenu");
        
        public void OnRestartButtonClicked() => LevelManager.RestartLevel();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
