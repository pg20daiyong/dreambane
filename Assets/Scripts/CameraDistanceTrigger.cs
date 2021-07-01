using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class CameraDistanceTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera; 
    [SerializeField] float cameraDistance;
    [SerializeField] float ScreenX, ScreenY;
    [SerializeField] float depthOfField;
    [SerializeField] private VolumeProfile _profile;

    private float _originalDistance;
    private float _originalScreenX, _originalScreenY;
    private float _originalDepthOfField;
    private void Awake()
    {
        CinemachineComponentBase componentBase = camera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            _originalDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance;
            _originalScreenX = (componentBase as CinemachineFramingTransposer).m_ScreenX;
            _originalScreenY = (componentBase as CinemachineFramingTransposer).m_ScreenY;

            if (_profile.TryGet(out DepthOfField depth))
            {
                _originalDepthOfField = depth.focusDistance.value;
            }

                
}
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CinemachineComponentBase componentBase = camera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            (componentBase as CinemachineFramingTransposer).m_CameraDistance = cameraDistance;
            (componentBase as CinemachineFramingTransposer).m_ScreenX = ScreenX;
            (componentBase as CinemachineFramingTransposer).m_ScreenY = ScreenY;
            if (_profile.TryGet(out DepthOfField depth))
            {
                depth.focusDistance.value = depthOfField;
            }
            
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CinemachineComponentBase componentBase = camera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            (componentBase as CinemachineFramingTransposer).m_CameraDistance = _originalDistance;
            (componentBase as CinemachineFramingTransposer).m_ScreenX = _originalScreenX;
            (componentBase as CinemachineFramingTransposer).m_ScreenY = _originalScreenY;
            if (_profile.TryGet(out DepthOfField depth))
            {
                depth.focusDistance.value = _originalDepthOfField;
            }
        }
    }
}
