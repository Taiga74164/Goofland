using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class UIController : MonoBehaviour
    {
        public PauseMenu pauseMenu;
        
        private InputAction _action;

        private void Start()
        {
            _action = InputManager.Return;
        }

        private void Update()
        {
            if (_action.WasReleasedThisFrame() && !GameManager.IsPaused && !pauseMenu.IsOpen)
                pauseMenu.OpenMenu();
        }
    }
}