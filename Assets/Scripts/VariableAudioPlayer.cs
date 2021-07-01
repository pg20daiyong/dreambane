using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableAudioPlayer : MonoBehaviour
{

    [SerializeField] private float slowThreshHold, mediumThreshHold, fastThreshHold;

    //[SerializeField] private AudioClip[] SlowSpillSounds;
    //[SerializeField] private AudioClip[] MediumSpillSounds;
    //[SerializeField] private AudioClip[] FastSpillSounds;
    [SerializeField] private AudioClip SpillSound;

    [SerializeField] private AudioSource source;

    [SerializeField] private float delayTime = 0.5f;

    private bool playingSound = false;

    public float variableRate { get; set; }

    // Update is called once per frame
    void Update()
    {
        //if (playingSound) return;
        //if(variableRate >= slowThreshHold)
        //{
        //    if (variableRate >= mediumThreshHold)
        //    {
        //        //highest speed
        //        if (variableRate >= fastThreshHold)
        //        {
        //            ChooseAudio(FastSpillSounds);
        //            return;
        //        }
        //        ChooseAudio(MediumSpillSounds);
        //        return;
        //    }
        //    ChooseAudio(SlowSpillSounds);
        //}

        if (variableRate <= slowThreshHold && playingSound)
        {
            source.Pause();
            playingSound = false;
        }

        if (variableRate > slowThreshHold && !playingSound)
        {
            source.Play();
            playingSound = true;
        }

        source.volume = Mathf.Clamp(variableRate*5, 0, 1);

    }

    private void ChooseAudio(AudioClip[] clips)
    {
        if(clips.Length == 0)
        {
            Debug.LogWarning("WARN- BUCKET AUDIO CLIPS MISSING");
            return;
        }
        AudioClip activeClip = clips[0];
        if (clips.Length > 1)
        {
            activeClip = clips[Random.Range(0, clips.Length)];
        }
        source.clip = activeClip;
        StartCoroutine(PlayAudio());
    }


    IEnumerator PlayAudio()
    {
        playingSound = true;
        source.Play();
        while(source.isPlaying)
        {
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(delayTime);
        playingSound = false;
    }
}
