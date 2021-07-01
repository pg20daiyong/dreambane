using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaterFreezeTimer : MonoBehaviour
{
    [SerializeField] public Image bucketFillImage;
    [SerializeField] public TextMeshProUGUI timeCounter;  

    [Header("Freeze Stage Time")]
    [SerializeField] private float freezeStage1 = 5f;
    [SerializeField] private float freezeStage2 = 10f;
    [SerializeField] private float freezeStage3 = 15f;
    [SerializeField] private float freezeStage4 = 20f;
    [SerializeField] public float TotalFreezeTime = 20f;

    [Header("Bucket Image Fill Amount")]
    [SerializeField] private float fillAmount = .25f;

    private TimeSpan timePlaying;    
    private AudioSource freezeSFX;
    private SimpleAudioPlayer clipsToPlay;
    private bool sfx1IsPlaying = false;
    private bool sfx2IsPlaying = false;
    private bool sfx3IsPlaying = false;
    private bool sfx4IsPlaying = false;

    public float ElapsedTime { get; private set; }
    public float FrozenPercent { get; set; }
    public bool TimerGoing { get;  set; }

    void Start()
    {       
        clipsToPlay = GetComponent<SimpleAudioPlayer>();
        TimerGoing = false;
        BeginTimer();
    }


    public void BeginTimer()
    {
        if (!GameMgr.instance.GameRunning) return;
        ElapsedTime = 0f;
        TimerGoing = true;
        sfx1IsPlaying = true;
        sfx2IsPlaying = true;
        sfx3IsPlaying = true;
        sfx4IsPlaying = true;

        StartCoroutine(UpdateTimer());
    }       
   
    private IEnumerator UpdateTimer()
    {
        
        //while timerGoing is true, timeCounter text is updated and bucketFillImage fill amount is increased based on elapsedTime and fillAmount      
        while (TimerGoing)
        {           
            ElapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(ElapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm' : 'ss' . 'ff");

            FrozenPercent = (ElapsedTime / TotalFreezeTime);

            if ((ElapsedTime > freezeStage1) && (ElapsedTime < freezeStage2))
            {
                if (sfx1IsPlaying == true)
                {
                    //clipsToPlay.Play();
                }               
                sfx1IsPlaying = false;
            }
            else if ((ElapsedTime > freezeStage2) && (ElapsedTime < freezeStage3))
            {
                if (sfx2IsPlaying == true)
                {
                    //clipsToPlay.Play();
                }
                sfx2IsPlaying = false;
            }
            else if ((ElapsedTime > freezeStage3) && (ElapsedTime < freezeStage4))
            {
                if (sfx3IsPlaying == true)
                {
                    //clipsToPlay.Play();
                }
                sfx3IsPlaying = false;
            }
            else if (ElapsedTime > freezeStage4)
            {
                if (sfx4IsPlaying == true)
                {
                    //clipsToPlay.Play();
                }
                sfx4IsPlaying = false;
            }
            else
            {
                yield return null;
            }
            
            yield return null;

        }
    }
}
