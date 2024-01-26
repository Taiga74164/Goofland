using UnityEngine;

namespace Levels
{
    public class DunkTank : MonoBehaviour, ITrigger
    {
        public Target dunkTarget;
        [Tooltip("The object to be dunked (e.g., enemy or person).")]
        public GameObject dunkedObject;
        [Tooltip("The position to which the dunked object should be moved.")]
        public Transform dunkedPosition;

        private void Start() => dunkedObject.SetActive(false);

        public void Trigger()
        {
            // Set the dunked object's position and activate it.
            dunkedObject.transform.position = dunkedPosition.position;
            dunkedObject.SetActive(true);

            // Disable the dunk target to prevent repeated dunking.
            dunkTarget.gameObject.SetActive(false);
        }
    }
}