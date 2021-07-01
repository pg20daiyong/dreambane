using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepController : MonoBehaviour
{
    private enum Surfaces
    {
        Snow,
        Ice,
        Stone,
    }

    [SerializeField] LayerMask targetLayers;
    [SerializeField] AudioClip[] snowFootSteps;
    [SerializeField] AudioClip[] iceFootSteps;
    [SerializeField] AudioClip[] stoneFootSteps;

    [SerializeField] private float _minAnimationWeight = 0.1f;

    private Surfaces curretSurface;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < _minAnimationWeight) return;

        SurfaceType();
    }

    private void SurfaceType()
    {
        RaycastHit hit;
        Debug.DrawLine(transform.position, -transform.up * 10, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0,.5f,0), -transform.up, out hit, 10f, targetLayers))
        {
            Debug.Log(hit.transform.tag);
            switch (hit.transform.tag)
            {
                case "Snow":
                    audioSource.clip = snowFootSteps[Random.Range(0, snowFootSteps.Length)];
                    audioSource.Play();
                    break;
                case "Ice":
                    audioSource.clip = iceFootSteps[Random.Range(0, iceFootSteps.Length)];
                    audioSource.Play();
                    break;
                case "Stone":
                    audioSource.clip = stoneFootSteps[Random.Range(0, stoneFootSteps.Length)];
                    audioSource.Play();
                    break;
            }
                
        }
    }
}
