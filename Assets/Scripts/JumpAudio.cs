using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAudio : MonoBehaviour
{

    [SerializeField] private AudioClip[] clips;
    
    [Range(0,100)]
    [SerializeField] private int percentChangeToPlay;

    [SerializeField] private AudioSource jumpAudioSource;

    public void PlayJumpAudio()
    {
        if (Random.Range(0,100) <= percentChangeToPlay)
        {
            jumpAudioSource.clip = clips[Random.Range(0, clips.Length)];
            jumpAudioSource.Play();
        }
    }
}

