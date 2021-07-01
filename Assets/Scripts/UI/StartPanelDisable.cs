using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelDisable : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private float _delayTime = 1f;

    public void DisablePanel()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayTime);
        _startPanel.SetActive(false);
    }
}
