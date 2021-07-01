using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{

    [SerializeField] private Transform target;
    private Transform headAim;
    private void Awake()
    {
        headAim = GameObject.Find("TurnTarget").GetComponent<Transform>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        headAim.position = target.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 defualtPos = new Vector3(0.01f, 0.165f, 0.484f);
            headAim.position = defualtPos;
    }
}
