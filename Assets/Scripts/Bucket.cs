using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bucket : MonoBehaviour
{
    [SerializeField] float FullWeight = 10f;
    [SerializeField] float soundEffectDelay = 2f;
    [SerializeField] Material waterMat;
    [SerializeField] VariableAudioPlayer audioPlayer;
    [SerializeField] ParticleSystem spillParticle;
    [SerializeField] float ParticleMultiplier = 500f;
    [SerializeField] WaterFreezeTimer timer;


    private bool playingSound = false;

    //will be taken directly from the timer component after update
    private float percentFrozen = 0;
    public float weight { get; private set; }
    public float waterRemaining { get; private  set; } = 100f;
    public bool isSpilling { get; set; } = false;

    public bool DebugEnabled { get; set; } = false;


    private void Awake()
    {
        UpdateProperties();
        spillParticle.Play(true);
    }

    private void Update()
    {

        UpdateProperties();

    }

    //sets weight depending on remaining water
    void UpdateProperties()
    {
        float fillPercent = (((waterRemaining / 100)/2.4f)-.50f)*-1f;
        weight = FullWeight * (waterRemaining / 100);
        waterMat.SetFloat("liquidFillLine", fillPercent);

        //TODO this needs to be changed so its dynamically figuring out how frozen it can be.
        percentFrozen = (timer.ElapsedTime / timer.TotalFreezeTime) * 100;

    }

    private void FixedUpdate()
    {
        if (!isSpilling || !GameMgr.instance.GameRunning)
        {
            spillParticle.enableEmission = false;
            audioPlayer.variableRate = 0;
        }
    }
    //Spills water
    public void SpillWater(float spilledAmmount)
    {
        if (DebugEnabled)
            return;

        if (waterRemaining <= percentFrozen)
            return;

        audioPlayer.variableRate = spilledAmmount;
        spillParticle.enableEmission = true;
        spillParticle.emissionRate = spilledAmmount * ParticleMultiplier;
        waterRemaining -= spilledAmmount;
        waterRemaining = Mathf.Clamp(waterRemaining, 0, 100f);

    }

    public void FillWater(float fillAmount)
    {
        Debug.Log("In bucket: " + fillAmount);
        waterRemaining += fillAmount;
        waterRemaining = Mathf.Clamp(waterRemaining, 0, 100f);
    }



}
