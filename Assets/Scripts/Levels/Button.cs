using UnityEngine;

namespace Levels
{
    public class Button : MonoBehaviour
    {
        public ITrigger trigger;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsPlayer())
                trigger.Trigger();
        }
    }
}
