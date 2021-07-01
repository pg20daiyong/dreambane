using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _startMenuResolutionDropdown;
    [SerializeField] private TMP_Dropdown _pauseMenuResolutionDropdown;

    Resolution[] _resolutions;

    private void Start()
    {
        _resolutions = Screen.resolutions;
        _startMenuResolutionDropdown.ClearOptions();
        _pauseMenuResolutionDropdown.ClearOptions();
        List<string> _resolutionOptions = new List<string>();
        int _currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string _option = _resolutions[i].width + " x " + _resolutions[i].height + ", " + _resolutions[i].refreshRate + "Hz";
            _resolutionOptions.Add(_option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }
        _startMenuResolutionDropdown.AddOptions(_resolutionOptions);
        _startMenuResolutionDropdown.value = _currentResolutionIndex;
        _startMenuResolutionDropdown.RefreshShownValue();
        _pauseMenuResolutionDropdown.AddOptions(_resolutionOptions);
        _pauseMenuResolutionDropdown.value = _currentResolutionIndex;
        _pauseMenuResolutionDropdown.RefreshShownValue();
    }
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int _resolutionIndex)
    {
        Resolution _resolution = _resolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);
    }
}
