using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpItembobbing : MonoBehaviour
{
    [Header("Item Bobbing")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _height = .5f;

    private Vector3 startPos;
    private Vector3 endPos;

    private float timeElapsed;
    private float totalTime = 5;
    private float _heigtOffset;

    public bool bobbing { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        _heigtOffset = transform.position.y;
        startPos = transform.position;
        endPos = startPos + new Vector3(0, _height, 0);
        StartBobbing();
    }

    public void StartBobbing()
    {
        if(!bobbing)
        {
            bobbing = true;
            StartCoroutine(Bobbing());
        }
    }

    public void StopBobbing()
    {
        if (bobbing)
        {
            bobbing = false;
        }
    }

    IEnumerator Bobbing()
    {
        _heigtOffset = transform.position.y;
        startPos = transform.position;
        endPos = startPos + new Vector3(0, _height, 0);
        timeElapsed = 0;

        Vector3 posOne, posTwo;
        posOne = startPos;
        posTwo = endPos;
        bool goingUp = true;
        while (bobbing)
        {
            if (timeElapsed >= totalTime)
            {
                if (goingUp)
                {
                    posOne = posTwo;
                    posTwo = startPos;
                    goingUp = false;
                }
                else
                {
                    posOne = startPos;
                    posTwo = endPos;
                    goingUp = true;
                }

                timeElapsed = 0;
            }
            timeElapsed += Time.deltaTime * _speed;
            float t = timeElapsed / totalTime;
            t = t * t * (3f - 2f * t);
            Debug.Log("Bobbing");
            transform.position = Vector3.Lerp(posOne, posTwo, t);
            yield return null;
        }
    }
}
