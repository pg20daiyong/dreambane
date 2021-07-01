using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSync : MonoBehaviour
{
    [SerializeField] private Toggle _statMenuToggle;
    [SerializeField] private Toggle _pauseMenuToggle;
    [SerializeField] private GameObject _startMenu;

    private void Update()
    {
        if (_startMenu.activeSelf == true)
        {
            if (_statMenuToggle.isOn == true)
            {
                _pauseMenuToggle.isOn = true;
            }
            else if (_statMenuToggle.isOn == false)
            {
                _pauseMenuToggle.isOn = false;
            }
        }
        else
        {
            return;
        }
    }
}
