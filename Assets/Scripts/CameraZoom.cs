using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    CinemachineComponentBase _componentBase;
    float cameraDistance;
    [SerializeField] float sens = 10f;

    private void Update()
    {
        if (_componentBase == null)
        {
            _componentBase = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sens;
            if(_componentBase is CinemachineFramingTransposer)
            {
                (_componentBase as CinemachineFramingTransposer).m_CameraDistance = Mathf.Clamp((_componentBase as CinemachineFramingTransposer).m_CameraDistance - cameraDistance, 1,15);
            }
        }
    }
}
