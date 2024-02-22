using UnityEngine.SceneManagement;

namespace Utils
{
    public static class LevelUtil
    {
        public static string CurrentLevelName => SceneManager.GetActiveScene().name;
        
        public static void LoadLevel(object levelIndex) => SceneManager.LoadScene((int)levelIndex);
        
        public static void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
        
        public static void RestartLevel() => LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
