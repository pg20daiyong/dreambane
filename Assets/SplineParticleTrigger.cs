using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class SplineParticleTrigger : MonoBehaviour
{
    [SerializeField] private bool DisableParticles = false;

    private bool _triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_triggered)
        {
            _triggered = true;
            if(DisableParticles)
            {
                SplineParticle.instance.StopEmmiting();
                return;
            }
            SplineParticle.instance.StopEmmiting();
        }
    }
}
