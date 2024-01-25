using UnityEngine;

namespace Levels
{
    public class Gate : MonoBehaviour
    {
        public void Open()
        {
            // Temporary.
            gameObject.SetActive(!gameObject.activeSelf);
        }
        
        public void Close()
        {
            // Temporary.
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}