using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class SplineMoveTrigger : MonoBehaviour
{

    [SerializeField] BezierSpline splineTarget;
    [SerializeField] float Duration;
    [SerializeField] SplineWalkerMode mode;


    [Header("Trigger controls")]
    [SerializeField] private bool TriggerOnce;
    [SerializeField] private bool TriggerDelay;


    private bool hasPlayed = false;


    private Collider collider;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!SplineParticle.instance.itemBobbing.bobbing) return;
            //returns if trigger once is enabled and it has played
            if (TriggerOnce && hasPlayed) return;
            hasPlayed = true;
            SplineParticle.instance.SetSpline(splineTarget, Duration, mode);
        }
    }


}

