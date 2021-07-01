using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPouring : MonoBehaviour
{

    [SerializeField] float pourThreshhold = 25f;
    [SerializeField] Transform origin = null;
    [SerializeField] GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;


    // Update is called once per frame
    void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshhold;
        
        if(isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring)
            {
                StartPour();
                return;
            }
            EndPour();
        }   
    }

    void StartPour()
    {
        print("Start Pour");
        currentStream = CreateStream();
        currentStream.Beguin();
    }

    void EndPour()
    {
        print("End Pour");
    }

    private float CalculatePourAngle()
    {
        return transform.forward.y * Mathf.Rad2Deg;
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}
