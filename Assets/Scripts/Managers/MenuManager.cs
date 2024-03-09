using System.Collections.Generic;
using UI;
using UnityEngine.EventSystems;

namespace Managers
{
    public class MenuManager : Singleton<MenuManager>
    {
        private Stack<Menu> _menuStack = new Stack<Menu>();
        
        /// <summary>
        /// Clears the menuStack.
        /// </summary>
        public void ClearStack() => _menuStack.Clear();
        
        /// <summary>
        /// Checks if the menuStack has any menus open and if the top menu is the same as the menu you want to check.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>true or false.</returns>
        public bool IsMenuOpen(Menu menu) => _menuStack.Count > 0 && _menuStack.Peek() == menu;
    
        /// <summary>
        /// Opens a specific menu and closes the previous menu.
        /// </summary>
        /// <param name="menu">The menu you want to close. This menu must inherit the Menu class</param>
        public void OpenMenu(Menu menu)
        {
            if (menu == null) return;
            
            // If the menu is already open and is the top menu, do nothing.
            if (_menuStack.Count > 0 && _menuStack.Peek().Equals(menu)) return;
            
            // Close all instances of the menu.
            while (_menuStack.Contains(menu))
                _menuStack.Pop().Close();
            
            // Close the previous menu.
            if (_menuStack.Count > 0)
                _menuStack.Peek().Close();
        
            // Open the menu.
            menu.Open();
            
            // Set the first selected object.
            EventSystem.current.SetSelectedGameObject(menu.firstSelected);
            
            // Add the menu to the stack.
            _menuStack.Push(menu);
        }
    
        /// <summary>
        /// Closes the top menu and opens the previous menu.
        /// </summary>
        public void CloseMenu()
        {
            if (_menuStack.Count == 0) return;
        
            // Close the top menu.
            var topMenu = _menuStack.Pop();
            topMenu.Close();
            
            // Open the previous menu.
            if (_menuStack.Count > 0)
            {
                _menuStack.Peek().Open();
                // Set the first selected object.
                EventSystem.current.SetSelectedGameObject(_menuStack.Peek().firstSelected);
            }
        }
    }
}