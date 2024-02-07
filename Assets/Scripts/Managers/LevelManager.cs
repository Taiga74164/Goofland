using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static void LoadLevel(object levelIndex) => SceneManager.LoadScene((int)levelIndex);
        
        public static void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
        
        public static void RestartLevel() => LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}

