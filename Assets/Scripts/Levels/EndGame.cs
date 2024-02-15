using UI;
using UI.Menus;
using UnityEngine;

namespace Levels
{
    public class EndGame : MonoBehaviour, ITrigger
    {
        public Victory victory;
        
        public void Trigger()
        {
            victory.OpenMenu();
        }
    }
}
