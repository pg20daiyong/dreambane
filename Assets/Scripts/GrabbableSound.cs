using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableSound : MonoBehaviour
{
    [SerializeField] float minimumVelocity = 0.01f;
    [Range(0,1)]
    [SerializeField] float maxVolume = 1;
    private Rigidbody _rb;
    private AudioSource _source;

    private bool isPlaying = false;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float velocity = _rb.velocity.magnitude/3*maxVolume;
        if (velocity <= minimumVelocity && isPlaying)
        {
            _source.Pause();
            isPlaying = false;
        }

        if(velocity > minimumVelocity && !isPlaying)
        {
            _source.Play();
            isPlaying = true;
        }

        _source.volume = Mathf.Clamp(velocity,0,maxVolume);

    }

    public void EnableGrabbing()
    {
        GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        GetComponent<FixedJoint>().breakTorque = Mathf.Infinity;
    }

    public void DisableGrabbing()
    {
        GetComponent<FixedJoint>().breakForce = 0;
        GetComponent<FixedJoint>().breakTorque = 0;
    }
}
