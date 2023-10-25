using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopper : Singleton<HitStopper>
{
    private float speed;
    private bool restoreTime;

    private void Start()
    {
        restoreTime = false;
    }

    private void Update()
    {
        if(restoreTime)
        {
            if(Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void StopTime(float changingTime, int restoreSpeed, float delay) 
    {
        speed = restoreSpeed;

        if(delay > 0f)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }

        Time.timeScale = changingTime;
    }

    IEnumerator StartTimeAgain(float amount)
    {
        yield return new WaitForSeconds(amount);
        restoreTime = true;
    }
}
