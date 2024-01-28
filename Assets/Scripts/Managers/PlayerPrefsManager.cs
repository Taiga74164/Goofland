using UnityEngine;

namespace Managers
{
    public static class PlayerPrefsManager
    {
        public static float BGMVolume
        {
            get => PlayerPrefs.GetFloat("BGMVolume", 1f);
            set => PlayerPrefs.SetFloat("BGMVolume", value);
        }
        
        public static float SFXVolume
        {
            get => PlayerPrefs.GetFloat("SFXVolume", 1f);
            set => PlayerPrefs.SetFloat("SFXVolume", value);
        }
        
        public static float MasterVolume
        {
            get => PlayerPrefs.GetFloat("MasterVolume", 1f);
            set => PlayerPrefs.SetFloat("MasterVolume", value);
        }

        public static void Save() => PlayerPrefs.Save();
    }
}