using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class AlphaClickBlocker : MonoBehaviour
    {
        private void Start() => GetComponent<Image>().alphaHitTestMinimumThreshold = 1.0f;
    }
}
