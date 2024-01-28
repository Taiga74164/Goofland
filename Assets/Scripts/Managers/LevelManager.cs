using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static void LoadLevel(int levelIndex) => SceneManager.LoadScene(levelIndex);
        
        public static void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
        
        public static void RestartLevel() => LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}

