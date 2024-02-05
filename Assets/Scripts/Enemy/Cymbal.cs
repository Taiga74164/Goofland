using Managers;
using Objects.Scriptable;
using UnityEngine;

public class Cymbal : Enemy
{
    public AudioData audioData;
    public AudioData deathAudioData;
    private AudioSource _audioSource;

    private readonly float _maxDistance = 10.0f;

    protected override void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Configure(audioData);
        _audioSource.Play();
        _audioSource.volume = 0.0f;
    }

    private void Update()
    {
        HandleProximity();
    }

    private void HandleProximity()
    {
        var playerTransform = GameManager.Instance.playerController.transform;
        var distance = Vector3.Distance(transform.position, playerTransform.position);
        var volume = Mathf.Clamp01(1 - distance / _maxDistance);
        _audioSource.volume = volume;
    }
    
    protected override void Die()
    {
        AudioManager.Instance.PlayAudio(deathAudioData);
        base.Die();
    }
}