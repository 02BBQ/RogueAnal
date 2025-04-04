using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera _cam;
    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
        _cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = _cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            StartCoroutine(Shaker());
        }
    }
    IEnumerator Shaker()
    {
        while (shakeTimer >= 0)
        {
            if(Time.timeScale>0f)
                shakeTimer -= Time.deltaTime* Time.timeScale;
            yield return null;
        }
        //timeover as fuck
        CinemachineBasicMultiChannelPerlin perlin = _cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = 0f;
    }
}
