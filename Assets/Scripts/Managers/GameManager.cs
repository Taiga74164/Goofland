using Controllers;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerController playerController;

        private bool _isPaused;
        public static bool IsPaused
        {
            get => Instance._isPaused;
            set => Instance._isPaused = value;
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        
        private static void OnSceneChanged(Scene current, Scene next)
        {
            // Unpause the game when the scene changes.
            IsPaused = false;
            // Clear the menu stack when the scene changes.
            MenuManager.Instance.ClearStack();
        }
    }
}
