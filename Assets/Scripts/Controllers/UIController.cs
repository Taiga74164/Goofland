using Managers;
using UI.Menus;
using UnityEngine;

namespace Controllers
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