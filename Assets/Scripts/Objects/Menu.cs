using Managers;

namespace Objects
{
    /// <summary>
    /// Represents a generic Singleton Menu.
    /// </summary>
    /// <typeparam name="T">Type of the specific menu class.</typeparam>
    public abstract class Menu<T> : Singleton<T> where T : Menu<T>
    {
        public void Open() => gameObject.SetActive(true);

        public void Close() => gameObject.SetActive(false);
    }

    /// <summary>
    /// Base class for all menus.
    /// </summary>
    public abstract class Menu : Menu<Menu>
    {
        protected virtual void Update()
        {
            HandleInput();
        }
    
        /// <summary>
        /// Checks if the menu is open.
        /// </summary>
        public bool IsOpen => MenuManager.Instance.IsMenuOpen(this);
    
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
            if (InputManager.Return.WasReleasedThisFrame())
                CloseMenu();
        }
    }
}