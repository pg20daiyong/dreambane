using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{

    [SerializeField] LayerMask ground;

    private LineRenderer lineRenderer;
    private Vector3 targetPosition = Vector3.zero;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);

    }

    public void Beguin()
    {
        StartCoroutine(BeguinPour());
    }

    private IEnumerator BeguinPour()
    {
        targetPosition = FindEndPoint();

        MoveToPosition(0, transform.position);
        MoveToPosition(1, targetPosition);

        yield return null;
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 2f, ground);
        Vector3 endPoint = hit.transform.position;
        Debug.DrawLine(transform.position, endPoint, Color.red, 15f);
        Debug.Log(hit.transform.position);
        return endPoint;
    }

    private void MoveToPosition(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }
}

