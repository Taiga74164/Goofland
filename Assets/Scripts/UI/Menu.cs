using JetBrains.Annotations;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// Base class for all menus.
    /// </summary>
    public abstract class Menu : MonoBehaviour
    {
        [Header("Menu Settings")]
        [CanBeNull] public GameObject firstSelected;
        
        protected virtual void Update()
        {
            HandleInput();
        }

        protected virtual void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        protected virtual void OnDisable()
        {
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        /// Checks if the menu is open.
        /// </summary>
        public bool IsOpen => MenuManager.Instance.IsMenuOpen(this);
        
        public void Open() => gameObject.SetActive(true);
        
        public void Close() => gameObject.SetActive(false);
        
        /// <summary>
        /// Requests the MenuManager to open the menu.
        /// </summary>
        public void OpenMenu() => MenuManager.Instance.OpenMenu(this);
    
        /// <summary>
        /// Requests the MenuManager to close the menu.
        /// </summary>
        public void CloseMenu() => MenuManager.Instance.CloseMenu();
    
        /// <summary>
        /// Defines the action to be taken when the back button is pressed.
        /// </summary>
        public virtual void OnBackPressed() => CloseMenu();
    
        /// <summary>
        /// Handles input specific to the menu.
        /// </summary>
        private void HandleInput()
        {
            if (InputManager.Return.WasReleasedThisFrame() || InputManager.Cancel.WasReleasedThisFrame())
                CloseMenu();
        }
    }
}