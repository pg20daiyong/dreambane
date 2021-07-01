using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingParticles : MonoBehaviour
{

    [SerializeField] ParticleSystem landingParticles;
    [SerializeField] LayerMask ground;

    void LandingParticle()
    {
        SurfaceType();
    }

    private void SurfaceType()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, .5f, 0), -transform.up, out hit, 10f, ground))
        {
            switch (hit.transform.tag)
            {
                case "Snow":
                    landingParticles.GetComponent<ParticleSystem>().Play();
                    break;
            }
        }
    }
}
