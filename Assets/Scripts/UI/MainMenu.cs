using Managers;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public string levelName = "Main";
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private Credits credits;

        public void OnPlayButtonClick() => LevelManager.LoadLevel(levelName);
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
        
        public void OnCreditsButtonClicked() => credits.OpenMenu();

        public void OnQuitButtonClicked()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
