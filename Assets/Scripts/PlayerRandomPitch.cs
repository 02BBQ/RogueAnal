using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomPitch : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [Range(0, 1)] public float pitch;

    private void OnEnable()
    {
        audioSource.pitch = Random.Range(-pitch, pitch) + 1;
        audioSource.Play();

    }
}
