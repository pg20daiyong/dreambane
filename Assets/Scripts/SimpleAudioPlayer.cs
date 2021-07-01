using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// forces AudioSource component to sit on same GameObject
[RequireComponent(typeof(AudioSource))]
public class SimpleAudioPlayer : MonoBehaviour
{
    [SerializeField] private bool _playOnAwake = false;
    [SerializeField] private AudioClip[] _clips = new AudioClip[0];
    [SerializeField] private Vector2 _volumeRange = new Vector2(0.8f, 1f);
    [SerializeField] private Vector2 _pitchRange = new Vector2(0.8f, 1f);

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        if(_playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        // randomize volume and pitch before playing
        _audioSource.volume = Random.Range(_volumeRange.x, _volumeRange.y);
        _audioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);

        // optionally randomize played clip
        if(_clips.Length > 0)
        {
            // assign random audio clip from array
            _audioSource.clip = _clips[Random.Range(0, _clips.Length)];
        }

        // play audio
        _audioSource.Play();
    }
}
