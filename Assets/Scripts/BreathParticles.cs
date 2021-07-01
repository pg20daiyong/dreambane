using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem breath;
    [SerializeField] private float breathRate = 5f;
    private bool inCheckpoint = false;


    private void Start()
    {
        StartCoroutine("Breathe");
    }

    private void Update()
    {
        if (!breath.isPlaying)
            breath.Play();
    }

    IEnumerator Breathe()
    {
        while(GameMgr.instance.GameRunning)
        {
            yield return new WaitForSeconds(breathRate);
            if (!inCheckpoint)
            {
                breath.Play();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
            inCheckpoint = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
            inCheckpoint = false;
    }
}
