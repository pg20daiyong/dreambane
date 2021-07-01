using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAudioPlayer : MonoBehaviour
{

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
