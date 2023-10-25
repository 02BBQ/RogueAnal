using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepper : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepAudioClip;
    private void Start()
    {
        print("stillalive");
    }
    public void OnFootStep(int ch)
    {
        audioSource.PlayOneShot(stepAudioClip[ch]);
    }
}
