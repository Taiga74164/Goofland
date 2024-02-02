using Managers;
using UnityEngine;
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
            bgmVolumeBar.onValueChanged.AddListener(OnBGMVolumeBarValueChanged);
            sfxVolumeBar.onValueChanged.AddListener(OnSFXVolumeBarValueChanged);
            masterVolumeBar.onValueChanged.AddListener(OnMasterVolumeBarValueChanged);
            
            LoadSettings();
            UpdateUIElements();
            UpdateSettings();
        }
        
        private void OnEnable() => GameManager.IsPaused = true;
        
        private void OnDisable() => GameManager.IsPaused = false;
    
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
            PlayerPrefsManager.Save();
        }
    
        public void OnBGMVolumeBarValueChanged(float value)
        {
            _bgmVolume = value;
            AudioManager.Instance.SetBGMVolume(bgmVolumeBar.value);
            SaveSettings();
        }
    
        public void OnSFXVolumeBarValueChanged(float value)
        {
            _sfxVolume = value;
            AudioManager.Instance.SetSFXVolume(sfxVolumeBar.value);
            SaveSettings();
        }
    
        public void OnMasterVolumeBarValueChanged(float value)
        {
            _masterVolume = value;
            AudioManager.Instance.SetMasterVolume(masterVolumeBar.value);
            SaveSettings();
        }

        public void OnExitButtonClicked() => CloseMenu();
    }
}
