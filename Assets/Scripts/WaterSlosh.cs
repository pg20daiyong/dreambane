using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlosh : MonoBehaviour
{

    [SerializeField] private float maxSlosh = 0.03f;
    [SerializeField] private float sloshSpeed = 1f;
    [SerializeField] private float ResetTime = 1f;

    private Renderer renderer;
    private Vector3 velocity;
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    private Vector3 angularVelocity;

    private float zMovement;
    private float xMovement;
    private float xForceToAdd;
    private float zForceToAdd;
    private float force;
    private float timePassed = 0.5f;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    private void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        // decrease wave over time
        xForceToAdd = Mathf.Lerp(xForceToAdd, 0, Time.deltaTime * (ResetTime));
        zForceToAdd = Mathf.Lerp(zForceToAdd, 0, Time.deltaTime * (ResetTime));


        // make a sine wave of the decreasing wobble
        force = 2 * Mathf.PI * sloshSpeed;
        zMovement = xForceToAdd * Mathf.Sin(force * timePassed);
        xMovement = zForceToAdd * Mathf.Sin(force * timePassed);

        //Update shader variables
        renderer.material.SetFloat("zMovement", zMovement);
        renderer.material.SetFloat("xMovement", xMovement);

        //set velocity based on how far the object has moved since last update
        velocity = (previousPosition - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - previousRotation;


        // add clamped velocity to wobble
        xForceToAdd += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * maxSlosh, -maxSlosh, maxSlosh);
        zForceToAdd += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * maxSlosh, -maxSlosh, maxSlosh);

        // keep last position
        previousPosition = transform.position;
        previousRotation = transform.rotation.eulerAngles;
    }
}