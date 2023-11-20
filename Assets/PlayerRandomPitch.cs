using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomPitch : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource.Play();
    }
}
