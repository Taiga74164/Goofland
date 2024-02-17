using UnityEngine;

namespace Levels
{
    public class Button : MonoBehaviour
    {
        [SerializeField, InterfaceType(typeof(ITrigger))]
        private Object myObject;
        private ITrigger Trigger => myObject as ITrigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsPlayer())
                Trigger.Trigger();
        }
    }
}
