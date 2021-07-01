using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectMgr : Singleton<AudioEffectMgr>
{
    public AudioClip _audioClip;
    public AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Plays()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
