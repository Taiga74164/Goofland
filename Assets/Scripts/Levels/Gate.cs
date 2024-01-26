using UnityEngine;

namespace Levels
{
    public class Gate : MonoBehaviour, ITrigger
    {
        public void Trigger()
        {
            // Temporary.
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}