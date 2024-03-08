using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class DunkTank : MonoBehaviour, ITrigger
    {
        [Tooltip("Dunk Tank Settings")]
        [SerializeField] private Target dunkTarget;
        [Tooltip("The object to be dunked (e.g., enemy or person).")]
        [SerializeField] private GameObject dunkedObject;
        [Tooltip("The position to which the dunked object should be moved.")]
        [SerializeField] private Transform dunkedPosition;

        [Header("Audio Settings")]
        [SerializeField] private AudioData dunkAudioData;
        
        private void Start() => dunkedObject.SetActive(false);

        public void Trigger()
        {
            // Set the dunked object's position and activate it.
            dunkedObject.transform.position = dunkedPosition.position;
            dunkedObject.SetActive(true);

            // Disable the dunk target to prevent repeated dunking.
            dunkTarget.gameObject.SetActive(false);
            
            // Play the dunk sound.
            AudioManager.Instance.PlayOneShotAudio(dunkAudioData, transform.position, false);
        }
    }
}