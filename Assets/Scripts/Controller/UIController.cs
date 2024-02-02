using Managers;
using UI;
using UnityEngine;

namespace Controller
{
    public class UIController : MonoBehaviour
    {
        public PauseMenu pauseMenu;
        
        private void Update()
        {
            if (InputManager.Return.WasReleasedThisFrame() && !GameManager.IsPaused && !pauseMenu.IsOpen)
                pauseMenu.OpenMenu();
        }
    }
}