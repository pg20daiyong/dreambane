using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    
    [SerializeField] private GameObject liquid, liquidMesh;

    private float _moveSpeed = 50;
    private float _rotateSpeed = 15;

    private float differene = 25;


    void Update()
    {
        Slosh();

        liquidMesh.transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime, Space.Self);
    }


    private void Slosh()
    {
        Quaternion inverseRotation = Quaternion.Inverse(transform.rotation);

        Vector3 finalRotation = Quaternion.RotateTowards(liquidMesh.transform.localRotation, inverseRotation, _moveSpeed * Time.deltaTime).eulerAngles;

        finalRotation.x = ClampRotationValue(finalRotation.x, differene);
        finalRotation.z = ClampRotationValue(finalRotation.z, differene);

        liquidMesh.transform.localEulerAngles = finalRotation;
    }


    private float ClampRotationValue(float value, float difference)
    {
        float returnValue = 0.0f;

        if(value > 180)
        {
            returnValue = Mathf.Clamp(value, 360 - difference, 360);
        }
        else
        {
            returnValue = Mathf.Clamp(value, 0, difference);
        }
        return returnValue;
    }

}
