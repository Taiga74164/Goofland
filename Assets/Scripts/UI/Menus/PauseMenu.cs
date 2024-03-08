using Managers;
using UnityEngine;
using Utils;

namespace UI.Menus
{
    public class PauseMenu : Menu
    {
        [Header("Pause Menu")]
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private Credits credits;
        [SerializeField] private Controls controls;

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

        public void OnHomeButtonClicked()
            => TransitionManager.Instance.LoadScene("MainMenu", TransitionManager.TransitionType.ZoomAndFade);

        public void OnRestartButtonClicked() => LevelUtil.RestartLevel(TransitionManager.TransitionType.ZoomAndFade);

        public void OnPlayButtonClicked() => CloseMenu();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
        
        public void OnCreditsButtonClicked() => credits.OpenMenu();
        
        public void OnControlsButtonClicked() => controls.OpenMenu();
    }
}
