using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private float _pauseDelayTime = .5f;

    private bool _isPausing;
    private bool _fuck;

    private void Start()
    {
        _isPausing = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        Debug.Log(Time.timeScale);
    }

    public void Pause()
    {
        Debug.Log("Fuck");
        if (!_isPausing)
        {
            _isPausing = true;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(PauseDelay());
        }
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(UnpauseDelay());        
    }

    private IEnumerator PauseDelay()
    {
        yield return new WaitForSeconds(_pauseDelayTime);
        _fuck = true;
        Time.timeScale = 0f;
        Debug.Log("Less Fuck?");
    }

    private IEnumerator UnpauseDelay()
    {
        while (!_fuck)
        {
            Debug.Log("Maybe Fuck?");
            yield return null;
        }

        Time.timeScale = 1f;
        _fuck = false;

        _isPausing = false;
    }
       
}
