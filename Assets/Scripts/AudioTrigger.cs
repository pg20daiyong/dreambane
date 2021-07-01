using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClip;

    [Header("General Controls")]
    [SerializeField] private bool playOnAwake = false;

    [Header("Looping Controls")]
    [SerializeField] private bool playOnce = true;
    [SerializeField] private float minDelay, maxDelay;
    [SerializeField] private bool looping = false;
    [SerializeField] private bool loopForever = false;
    [SerializeField] private int numberOfLoops = 1;

    private AudioSource source;
    private bool hasPlayed = false;
    private bool isPlaying = false;

    private void Awake()
    {
        source = GetComponentInChildren<AudioSource>();
        //force collider to always be a trigger

        if(GetComponent<BoxCollider>() != null)  
            GetComponent<BoxCollider>().isTrigger = true;

        if (GetComponent<SphereCollider>() != null)
            GetComponent<SphereCollider>().isTrigger = true;

        if (GetComponent<CapsuleCollider>() != null)
            GetComponent<CapsuleCollider>().isTrigger = true;

        if (playOnAwake && audioClip.Length != 0) 
        {
            AudioClip activeClip = audioClip[0];
            if (audioClip.Length > 1)
            {
                activeClip = audioClip[Random.Range(0, audioClip.Length)];
            }
            source.clip = activeClip;

            StartCoroutine(PlayAudio());
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (hasPlayed && playOnce)
            return;

        //returns if no audio was attached 
        if (audioClip.Length == 0)
        {
            Debug.Log("Missing audio source in " + gameObject.transform.name);
            return;
        }


        if (other.CompareTag("Player"))
        {
            AudioClip activeClip = audioClip[0];
            if(audioClip.Length > 1)
            {
                activeClip = audioClip[Random.Range(0, audioClip.Length)];
            }
            source.clip = activeClip;
            hasPlayed = true;

            if (!isPlaying)
                StartCoroutine(PlayAudio());
        }
    }

    //starts timer for loop
    IEnumerator PlayAudio()
    {
        isPlaying = true;
        int loops = 0;
        do
        {
            if(!loopForever)
                loops++;

            source.Play();
            //randomize while looping
            if (playOnAwake && audioClip.Length != 0)
            {
                AudioClip activeClip = audioClip[0];
                if (audioClip.Length > 1)
                {
                    activeClip = audioClip[Random.Range(0, audioClip.Length)];
                }
                source.clip = activeClip;
            }
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

        } while (looping && loops <= numberOfLoops);
        isPlaying = false;
    }
}
