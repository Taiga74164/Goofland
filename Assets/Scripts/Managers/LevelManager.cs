using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static void LoadLevel(object levelIndex) => SceneManager.LoadScene((int)levelIndex);
        
        public static void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
        
        public static void RestartLevel()
        {
            // Clear the stack of menus before reloading the level.
            MenuManager.Instance.ClearStack();
            // Unpause the game.
            if (GameManager.IsPaused) GameManager.IsPaused = false;
            // Reload the current level.
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

