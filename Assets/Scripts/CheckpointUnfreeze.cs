using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CheckpointUnfreeze : MonoBehaviour
{    
    //[SerializeField] private TextMeshProUGUI timeCounterReverse;

    [Header("Freeze Stage Time")]
    [SerializeField] private float unFreezeStage1 = 5f;
    [SerializeField] private float unFreezeStage2 = 10f;
    [SerializeField] private float unFreezeStage3 = 15f;
    [SerializeField] private float unFreezeStage4 = 20f;    

    [Header("Bucket Image Unfill Amount")]
    [SerializeField] private float unFillAmount = .25f;
    

    private TimeSpan timePlaying;
    private float elapsedReverseTime;
    private bool reverseTimerGoing = false;

    [Header("Object with WaterFreezeTimer Script")]
    public WaterFreezeTimer wFT;
    
    public float CurrentFill { get => wFT.bucketFillImage.fillAmount; set => CurrentFill = value; }


    //when an object enters the trigger, the wFT.timeGoing is set to false and BeginReverseTimer fucntion is called
    private void OnTriggerEnter(Collider other)
    {
        if (wFT.TimerGoing == true && other.CompareTag("Player"))
        {
            wFT.TimerGoing = false;
            BeginReverseTimer();
        }
    }

    //when object exits trigger, wFT.timeGoing is set to true and wFT.BeginTimer is called
    private void OnTriggerExit(Collider other)
    {
        if (wFT.TimerGoing == false && other.CompareTag("Player"))
        {
            reverseTimerGoing = false;
            wFT.BeginTimer();            
        }
    }   

    public void BeginReverseTimer()
    {
        elapsedReverseTime = 0f;
        reverseTimerGoing = true;        

        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (reverseTimerGoing == true)
        {
            elapsedReverseTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedReverseTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm' : 'ss' . 'ff");

            wFT.FrozenPercent = wFT.FrozenPercent + ((elapsedReverseTime / 20)*-1);



            yield return null; 
        }
    }   

}
