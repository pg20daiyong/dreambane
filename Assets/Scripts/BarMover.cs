using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMover : MonoBehaviour
{
    [SerializeField] GameObject bar;
    [SerializeField] float horizontal = 0.0f;
    [SerializeField] float vertical = 0.1f;


    //Transform previousPosition;
    //Transform currentPosition;
    //Transform diffPosition;
    //private float scala = 1.25f;
    //private float zScala = 1.06f;


    void Update()
    {

        bar.transform.position = Camera.main.WorldToScreenPoint((transform.position + new Vector3(0.0f + horizontal, 0.1f + vertical, 0)));

        //bar.transform.position = Camera.main.WorldToScreenPoint((
        //    new Vector3(transform.position.x * scala, transform.position.y, transform.position.z * zScala) + 
        //    new Vector3(-0.7f, 0.7f, 0)));
        //Debug.Log(bar.name+"  "+transform.position);
    }
}
