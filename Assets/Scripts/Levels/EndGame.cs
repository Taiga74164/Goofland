using UI;
using UnityEngine;

namespace Levels
{
    public class EndGame : MonoBehaviour, ITrigger
    {
        public Victory victory;
        
        public void Trigger()
        {
            Debug.Log("end game");
            victory.OpenMenu();
        }
    }
}
