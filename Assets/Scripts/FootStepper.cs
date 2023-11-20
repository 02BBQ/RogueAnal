using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepper : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepAudioClip;
    public void OnFootStep(int ch)
    {
        audioSource.PlayOneShot(stepAudioClip[ch]);
    }
}
