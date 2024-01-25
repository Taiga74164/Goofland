using UnityEngine;

namespace Levels
{
    public class Button : MonoBehaviour
    {
        public Gate gate;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsPlayer())
                gate.Open();
        }
    }
}
