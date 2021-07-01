using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupTrigger : MonoBehaviour
{
    [Header("Must Be On A Canvas")]
    [SerializeField] private GameObject _imageToEnable;
    [SerializeField] private float _timeUntilDisabled = 3f;
    [SerializeField] private float _timeUntilStart = 3f;
    private bool active = false;


    // on trigger enter, set _imageToEnable.enabled to true then call coroutine
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && GameMgr.instance.GameRunning)
    //    {
    //        Debug.Log("Enabled");
    //        _imageToEnable.SetActive(true);
    //        //StartCoroutine(ExecuteAfterTime(_timeUntilDisabled));
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && GameMgr.instance.GameRunning && !GameMgr.instance.GamePaused)
        {
            Debug.Log("Active");
            if (!active)
            {
                active = true;
                StartCoroutine(ExecuteDuringTime());
            }
        }
        else
        {
            Debug.Log("Not Active");
            _imageToEnable.SetActive(false);
            active = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_imageToEnable.active)
        {
            StartCoroutine(ExecuteAfterTime(_timeUntilDisabled));
        }
    }

    // waits for _time then sets _imageToEnable.enabled to false
    IEnumerator ExecuteAfterTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        _imageToEnable.SetActive(false);
        Debug.Log("Not Active");
        active = false;
    }

    IEnumerator ExecuteDuringTime()
    {
        yield return new WaitForSeconds(_timeUntilStart);
        _imageToEnable.SetActive(true);
        Debug.Log("Not Active");

    }
}
