using Managers;
using Objects.Scriptable;
using UnityEngine;
using Utils;

/// <summary>
/// Script for triggering audio clips based on collision or trigger events.
/// </summary>
public class InteractiveAudioTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioData audioData;
    [SerializeField] private float triggerChance = 0.5f;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Scene specific audio trigger.
        if (!LevelUtil.CurrentLevelName.Contains("MintageUrban")) return;
        if (!other.IsPlayer() && !other.gameObject.CompareTag("Enemy")) return;
        if (Random.value <= triggerChance)
            AudioManager.Instance.PlayOneShotAudio(audioData, transform.position, false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Scene specific audio trigger.
        if (!LevelUtil.CurrentLevelName.Contains("MintageUrban")) return;
        if (!other.IsPlayer() && !other.gameObject.CompareTag("Enemy")) return;
        if (Random.value <= triggerChance)
            AudioManager.Instance.PlayOneShotAudio(audioData, transform.position, false);
    }
}