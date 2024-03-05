using Managers;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
    public class SettingsMenu : Menu
    {
        public Slider bgmVolumeBar;
        public Slider sfxVolumeBar;
        public Slider masterVolumeBar;
    
        private float _bgmVolume;
        private float _sfxVolume;
        private float _masterVolume;

        private void Awake()
        {
            bgmVolumeBar.onValueChanged.AddListener(OnBGMVolumeBarValueChanged);
            sfxVolumeBar.onValueChanged.AddListener(OnSFXVolumeBarValueChanged);
            masterVolumeBar.onValueChanged.AddListener(OnMasterVolumeBarValueChanged);
            
            LoadSettings();
            UpdateUIElements();
            UpdateSettings();
        }

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
    
        private void LoadSettings()
        {
            _bgmVolume = PlayerPrefsUtil.BGMVolume;
            _sfxVolume = PlayerPrefsUtil.SFXVolume;
            _masterVolume = PlayerPrefsUtil.MasterVolume;
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
            PlayerPrefsUtil.BGMVolume = _bgmVolume;
            PlayerPrefsUtil.SFXVolume = _sfxVolume;
            PlayerPrefsUtil.MasterVolume = _masterVolume;
            PlayerPrefsUtil.Save();
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
    }
}
