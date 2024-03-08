using Controllers;
using Managers;
using Objects.Scriptable;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    [SerializeField] private AudioData pickupAudioData;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>()) return;
        
        AudioManager.Instance.PlayOneShotAudio(pickupAudioData, transform.position, false);
        other.GetComponent<PlayerController>().HasUmbrella = true;
        Destroy(gameObject);
    }
}
