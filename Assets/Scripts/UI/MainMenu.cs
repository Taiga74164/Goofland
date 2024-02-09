using Managers;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public string levelName = "Main";
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private Credits credits;
        
        #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void CloseTab();
        #endif
        
        public void OnPlayButtonClick() => LevelManager.LoadLevel(levelName);
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
        
        public void OnCreditsButtonClicked() => credits.OpenMenu();

        public void OnQuitButtonClicked()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBGL
            CloseTab();
            #else
            Application.Quit();
            #endif
        }
    }
}
