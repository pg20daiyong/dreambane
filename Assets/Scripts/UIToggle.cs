using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToggle : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    private bool _enabled = false;

    public bool Toggle()
    {
        if(_enabled)
        {
            _enabled = false;
            text.text = "Off";
            return false;
        }
        _enabled = true;
        text.text = "On";
        return true;
    }
}
