using UnityEngine;

namespace Levels
{
    public class Target : MonoBehaviour
    {
        public GameObject trigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Pie>())
                trigger.GetComponent<ITrigger>().Trigger();
        }
    }
}
