using UnityEngine;

namespace Levels
{
    public class Target : MonoBehaviour
    {
        public Gate gate;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Pie>())
                gate.Open();
        }
    }
}
