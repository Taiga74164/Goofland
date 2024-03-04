using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TabMenu : MonoBehaviour
    {
        public UDictionary<GameObject, GameObject> tabDictionary;

        private GameObject _currentTabContent;
        
        private void OnEnable()
        {
            if (_currentTabContent == null && tabDictionary.Count > 0)
                SetTab();
            else
                DisplayTabContent();
        }
        
        /// <summary>
        /// Set the first tab as the current tab.
        /// </summary>
        public void SetTab()
        {
            // Set the first tab as the current tab
            var currentTabButton = EventSystem.current.currentSelectedGameObject;
            if (currentTabButton == null)
                currentTabButton = tabDictionary.Keys[0];
            
            // Return if the tab is not in the dictionary or the tab is already the current tab
            if (!tabDictionary.ContainsKey(currentTabButton!) || tabDictionary[currentTabButton] == _currentTabContent)
                return;
            
            // Set the current tab and update the tab
            _currentTabContent = tabDictionary[currentTabButton];
            DisplayTabContent();
        }
        
        /// <summary>
        /// Display the current tab content and hide the rest.
        /// </summary>
        private void DisplayTabContent() =>
            tabDictionary.Values.ForEach(tab => tab.SetActive(tab == _currentTabContent));
    }
}