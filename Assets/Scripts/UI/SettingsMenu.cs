using Managers;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : Menu
    {
        public Slider bgmVolumeBar;
        public Slider sfxVolumeBar;
        public Slider masterVolumeBar;
    
        private float _bgmVolume;
        private float _sfxVolume;
        private float _masterVolume;

        protected override void Awake()
        {
            LoadSettings();
            UpdateUIElements();
            UpdateSettings();
        }
    
        private void LoadSettings()
        {
            _bgmVolume = PlayerPrefsManager.BGMVolume;
            _sfxVolume = PlayerPrefsManager.SFXVolume;
            _masterVolume = PlayerPrefsManager.MasterVolume;
        }
    
        private void UpdateUIElements()
        {
            bgmVolumeBar.SetValueWithoutNotify(_bgmVolume);
            sfxVolumeBar.SetValueWithoutNotify(_sfxVolume);
            masterVolumeBar.SetValueWithoutNotify(_masterVolume);
        }
    
        private void UpdateSettings()
        {
            OnBGMVolumeBarValueChanged(_bgmVolume);
            OnSFXVolumeBarValueChanged(_sfxVolume);
            OnMasterVolumeBarValueChanged(_masterVolume);
        }
    
        private void SaveSettings()
        {
            PlayerPrefsManager.BGMVolume = _bgmVolume;
            PlayerPrefsManager.SFXVolume = _sfxVolume;
            PlayerPrefsManager.MasterVolume = _masterVolume;
        }
    
        public void OnBGMVolumeBarValueChanged(float value)
        {
            PlayerPrefsManager.BGMVolume = value;
            SaveSettings();
        }
    
        public void OnSFXVolumeBarValueChanged(float value)
        {
            PlayerPrefsManager.SFXVolume = value;
            SaveSettings();
        }
    
        public void OnMasterVolumeBarValueChanged(float value)
        {
            PlayerPrefsManager.MasterVolume = value;
            SaveSettings();
        }

        public void OnExitButtonClicked() => CloseMenu();
    }
}
