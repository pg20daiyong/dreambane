using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yoke : MonoBehaviour
{

    public static Yoke instance = null;

    [SerializeField] float zMinClamp, zMaxClamp;
    [SerializeField] float xMinClamp, xMaxClamp;

    [Header("Bucket Weights")]
    [SerializeField] Bucket leftBucket;
    [SerializeField] Bucket rightBucket;
    [Header("Spill Rate")]
    [SerializeField] private float spillRateMultiplier = 0.1f;
    [SerializeField] private float angleSpillRateMultiplier = 0.1f;
    [Header("Weight Sensitivity")]
    [SerializeField] private float weightSensitivity = .5f;

    private float spillRate;
    private float xRotatonRate,yRotationRate;
    private Vector3 currentRotation;

    public float yRotation { get; set; } = 90;
    public bool isGrabbing { get; set; } = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }

    public void FixedUpdate()
    {
        if (!GameMgr.instance.GameRunning || GameMgr.instance.GamePaused) return;
        checkWeights();

        transform.Rotate(new Vector3(Mathf.LerpAngle(transform.localRotation.x, yRotationRate, 1 * Time.deltaTime), 0, Mathf.LerpAngle(transform.localRotation.z, xRotatonRate, 1) * Time.deltaTime)); 
        currentRotation = transform.rotation.eulerAngles;
        currentRotation = ClampEuelerAngles(currentRotation);
        if (isGrabbing)
        {
            //currentRotation = MoveToCenter(currentRotation);
        }
        transform.rotation = Quaternion.Euler(currentRotation);
        SpillRate();
    }

    //sets rotation rate with target direction
    public void Rotate(float xAngle, float yAngle)
    {
        xRotatonRate += xAngle;
        xRotatonRate = Mathf.Clamp(xRotatonRate, -45f, 45f);

        yRotationRate += yAngle;
        yRotationRate = Mathf.Clamp(yRotationRate, -45f, 45f);
    }

    //prevents yoke from rotating outside of clamp angles
    private Vector3 ClampEuelerAngles(Vector3 angle)
    {
        Vector3 tmpAngle = angle;
        if(angle.z > 0 && angle.z < zMinClamp)
        {
        }
        else if(angle.z > zMaxClamp && angle.z <= 360)
        {
        }
        else if(angle.z > zMinClamp && angle.z < 270)
        {
            tmpAngle.z = zMinClamp;
            
        }
        else if (angle.z < zMaxClamp && angle.z > 270)
        {
            tmpAngle.z = zMaxClamp;
        }

        if (angle.x > 0 && angle.x < xMinClamp)
        {
        }
        else if (angle.x > xMaxClamp && angle.x <= 360)
        {
        }
        else if (angle.x > xMinClamp && angle.x < 270)
        {
            tmpAngle.x = xMinClamp;

        }
        else if (angle.x < xMaxClamp && angle.x > 270)
        {
            tmpAngle.x = xMaxClamp;
        }

        tmpAngle.y = yRotation;
        if(isGrabbing)
        {
            tmpAngle.x = 0;
            tmpAngle.z = 0;
        }
        return tmpAngle;
    }

    //checks the weights of the buckets
    private void checkWeights()
    {
        if(!isGrabbing)
        {
            float difference = leftBucket.weight - rightBucket.weight;
            Rotate(difference * weightSensitivity, 0);
        }
    }

    //calls on the buckets to spill
    private void spilling(bool isLeft)
    {
        if (isLeft)
        {
            leftBucket.SpillWater(spillRate);
            leftBucket.isSpilling = true;
            return;
        }
        rightBucket.SpillWater(spillRate);
        rightBucket.isSpilling = true;
    }


    void SpillRate()
    {
        spillRate = 0f;
        if (transform.eulerAngles.z > 10 && transform.eulerAngles.z < 270)
        {
            spillRate = (transform.eulerAngles.z * angleSpillRateMultiplier) * (Mathf.Clamp((leftBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier;
            if (transform.eulerAngles.x > 30 && transform.eulerAngles.x < 270)
            {
                spillRate += (transform.eulerAngles.x * angleSpillRateMultiplier) * (Mathf.Clamp((leftBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier;
            }
            if (transform.eulerAngles.x < 355 && transform.eulerAngles.x > 270)
            {
                //spillRate += (transform.eulerAngles.x * angleSpillRateMultiplier) * (Mathf.Clamp((leftBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier;
            }
            spilling(true);
            return;
        }
        if (transform.eulerAngles.z < 340 && transform.eulerAngles.z > 270)
        {
            spillRate = (((transform.eulerAngles.z - 360) * -1) * angleSpillRateMultiplier) * (Mathf.Clamp((rightBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier;
            //spillRate = (transform.eulerAngles.z * angleSpillRateMultiplier) * (Mathf.Clamp((leftBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier;
            if (transform.eulerAngles.x > 30 && transform.eulerAngles.x < 270)
            {
                spillRate += (((transform.eulerAngles.z - 360) * -1) * angleSpillRateMultiplier) * (Mathf.Clamp((leftBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier / 4;
            }
            if (transform.eulerAngles.x < 340 && transform.eulerAngles.x > 270)
            {
                spillRate += (transform.eulerAngles.x * angleSpillRateMultiplier) * (Mathf.Clamp((leftBucket.waterRemaining / 100), 0.01f, 1)) * spillRateMultiplier / 4;
            }
            spilling(false);
            return;
        }
        StopSpilling();
    }


    void StopSpilling()
    {
        leftBucket.isSpilling = false;
        rightBucket.isSpilling = false;

    }
    //TODO make it smoothly move to the center when grabbing
    //Vector3 MoveToCenter(Vector3 currentAngle)
    //{
       
    //}

}
