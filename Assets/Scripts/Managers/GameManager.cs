using Controllers;
using UnityEngine;
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
            set
            {
                Instance._isPaused = value;
                // Time.timeScale = value ? 0.0f : 1.0f;
                // Instance.HandleRigidbodies(value);
            }
        }
        
        // private List<Rigidbody2D> _rigidbodies = new List<Rigidbody2D>();
        // private Dictionary<Rigidbody2D, Vector2> _velocities = new Dictionary<Rigidbody2D, Vector2>();
        // private Dictionary<Rigidbody2D, float> _angularVelocities = new Dictionary<Rigidbody2D, float>();

        protected override void OnAwake()
        {
            PrefabManager.Initialize();
            AudioManager.Initialize();
            InputManager.Initialize();
            TransitionManager.Initialize();
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
            // Reset currency when the scene changes.
            CurrencyManager.Instance.ResetCurrency();
        }
        
        // private void HandleRigidbodies(bool pause)
        // {
        //     if (pause)
        //     {
        //         _rigidbodies.Clear();
        //         _rigidbodies.AddRange(FindObjectsOfType<Rigidbody2D>());
        //         // Store the rigidbody velocities.
        //         foreach (var rb in _rigidbodies)
        //         {
        //             _velocities[rb] = rb.velocity;
        //             _angularVelocities[rb] = rb.angularVelocity;
        //             rb.velocity = Vector2.zero;
        //             rb.angularVelocity = 0.0f;
        //         }
        //     }
        //     else
        //     {
        //         // Restore the rigidbody velocities.
        //         foreach (var rb in _rigidbodies)
        //         {
        //             if (_rigidbodies.Contains(rb))
        //                 rb.velocity = _velocities[rb];
        //             if (_rigidbodies.Contains(rb))
        //                 rb.angularVelocity = _angularVelocities[rb];
        //         }
        //         
        //         _rigidbodies.Clear();
        //         _velocities.Clear();
        //         _angularVelocities.Clear();
        //     }
        // }
    }
}