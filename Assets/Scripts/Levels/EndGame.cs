using UI;
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
