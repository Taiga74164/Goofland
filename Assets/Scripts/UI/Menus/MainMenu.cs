using UnityEngine;
using Utils;

namespace UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LevelSelectMenu levelSelectMenu;
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private Credits credits;
        [SerializeField] private Controls controls;

        #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void CloseTab();
        #endif

        public void OnPlayButtonClick() => levelSelectMenu.OpenMenu();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
        
        public void OnCreditsButtonClicked() => credits.OpenMenu();
        
        public void OnControlsButtonClicked() => controls.OpenMenu();

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
