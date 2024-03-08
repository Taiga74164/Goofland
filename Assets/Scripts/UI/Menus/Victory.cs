using Managers;
using UnityEngine;
using Utils;

namespace UI.Menus
{
    public class Victory : Menu
    {
        [Header("Victory Menu")]
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
        
        public void OnHomeButtonClicked() => 
            TransitionManager.Instance.LoadScene("MainMenu", TransitionManager.TransitionType.ZoomAndFade);
        
        public void OnRestartButtonClicked() => LevelUtil.RestartLevel(TransitionManager.TransitionType.ZoomAndFade);
        
        public void OnSettingsButtonClicked() => settingsMenu.OpenMenu();
    }
}
