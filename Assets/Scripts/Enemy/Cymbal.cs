using Managers;
using Objects.Scriptable;
using UnityEngine;

public class Cymbal : Enemy
{
    public AudioData audioData;
    public AudioData deathAudioData;
    private AudioSource _audioSource;

    protected override void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Configure(audioData);
    }
    
    protected override void Die()
    {
        AudioManager.Instance.PlayAudio(deathAudioData);
        base.Die();
    }
}