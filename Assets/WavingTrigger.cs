using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingTrigger : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] Transform kitobrineTransform;

    private bool _waving;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        animator.SetBool("Waving", true);
        _waving = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        animator.SetBool("Waving", false);
        _waving = false;
    }

    private void Update()
    {
        if(_waving)
        {
            kitobrineTransform.LookAt(GameMgr.instance.KitLocation.position);
        }
    }
}
