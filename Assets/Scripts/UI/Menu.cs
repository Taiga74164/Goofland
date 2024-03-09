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
        
        private GameObject _lastSelected;
        
        protected virtual void Update()
        {
            HandleInput();
        }

        protected virtual void OnEnable()
        {
            // Keep track of the last selected game object.
            _lastSelected = EventSystem.current.currentSelectedGameObject;
            // Set the first selected game object if it's not null.
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        protected virtual void OnDisable()
        {
            // Set the last selected game object if it's not null.
            if (_lastSelected != null) EventSystem.current.SetSelectedGameObject(_lastSelected);
            // Clear the last selected game object.
            _lastSelected = null;
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