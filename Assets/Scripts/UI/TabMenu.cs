using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TabMenu : Menu
    {
        [SerializeField] private List<GameObject> tabs;
        [SerializeField] private List<GameObject> tabPanels;

        private int _currentTab;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetTab(_currentTab);
        }
        
        /// <summary>
        /// Activates the tab at the given index.
        /// </summary>
        /// <param name="index">The index of the tab to activate.</param>
        public void SetTab(int index)
        {
            // If the index is out of range or the same as the current tab, return.
            if (index < 0 || index >= tabs.Count || index == _currentTab) return;
            
            // Deactivate all tab panels and activate the one at the given index.
            tabPanels.ForEach(panel => panel.SetActive(false));
            tabPanels[index].SetActive(true);
            
            // Update the current tab.
            _currentTab = index;
        }
        
        /// <summary>
        /// Activates the next tab.
        /// </summary>
        public void NextTab()
        {
            var nextTab = (_currentTab + 1) % tabs.Count;
            SetTab(nextTab);
        }
        
        /// <summary>
        /// Activates the previous tab.
        /// </summary>
        public void PreviousTab()
        {
            var previousTab = (_currentTab - 1 + tabs.Count) % tabs.Count;
            SetTab(previousTab);
        }
    }
}