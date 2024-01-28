using UnityEngine;

namespace Levels
{
    public class Target : MonoBehaviour
    {
        [SerializeField, InterfaceType(typeof(ITrigger))]
        private Object myObject;
        private ITrigger Trigger => myObject as ITrigger;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Pie>())
                Trigger.Trigger();
        }
    }
}
