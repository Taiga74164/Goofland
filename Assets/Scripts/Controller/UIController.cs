using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class UIController : MonoBehaviour
    {
        public PauseMenu pauseMenu;
        
        // private InputAction _action;
        //
        // private void Start()
        // {
        //     _action = InputManager.Return;
        //     _action.canceled += _ => PauseGame();
        // }
        
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && !GameManager.Instance.IsPaused && !pauseMenu.IsOpen)
                pauseMenu.OpenMenu();
        }
    }
}