using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class HitStopper : Singleton<HitStopper>
{
    public Volume bloomVolume;
    private Bloom bloom;

    private void Start()
    {
        if (bloomVolume.profile.TryGet<Bloom>(out bloom))
        {
            //bloom.intensity.value = 500;
        }
    }

    public void StopTime(float changingTime, float delay) 
    {
        StartCoroutine(StartTimeAgain(changingTime, delay));
    }

    IEnumerator StartTimeAgain(float changingTime, float delay)
    {
        Time.timeScale = changingTime;
        bloom.intensity.value += 20f;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        bloom.intensity.value -= 20f;
    }

}
