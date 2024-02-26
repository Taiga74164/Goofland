using Managers;
using Objects;
using UnityEngine;
using Utils;

namespace UI.Menus
{
    public class PauseMenu : Menu
    {
        [SerializeField] private SettingsMenu settingsMenu;

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

        public void OnRestartButtonClicked() => LevelUtil.RestartLevel();

        public void OnPlayButtonClicked() => CloseMenu();
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
