using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinToss;
    [SerializeField] AudioClip coinSpin;
    [SerializeField] AudioClip head;
    [SerializeField] AudioClip tail;

    [SerializeField] ParticleSystem headFX;
    [SerializeField] ParticleSystem tailFX;

    [SerializeField] AudioSource audioSource;

    private bool isHead = false;

    private void OnEnable()
    {
        StartCoroutine(flip());
    }



    IEnumerator flip()
    {
        audioSource.PlayOneShot(coinToss);
        yield return new WaitForSeconds(.2f);
        audioSource.PlayOneShot(coinSpin);
        int i = Random.Range(1, 4);
        if (Random.Range(1, 3) == 1)
        {
            isHead = true;
        }
        else
            isHead = false;
        yield return new WaitForSeconds(1.25f);
        if (isHead)
        {
            audioSource.PlayOneShot(head);
            headFX.Play();

            switch (i)
            {
                case 1:
                    Player.Instance.Health++;
                    break;
                case 2:
                    float dmgTemp = Player.Instance.dmg / 2f;
                    Player.Instance.dmg += dmgTemp;
                    yield return new WaitForSeconds(5);
                    Player.Instance.dmg -= dmgTemp;
                    break;
                case 3:
                    float temp = Player.Instance.GetComponent<Movement>().moveSpeed / 4f;
                    Player.Instance.GetComponent<Movement>().moveSpeed += temp;
                    yield return new WaitForSeconds(5);
                    Player.Instance.GetComponent<Movement>().moveSpeed -= temp;
                    break;
                default:
                    break;
            }
        }
        else
        {
            audioSource.PlayOneShot(tail);
            tailFX.Play();
            
            switch (i)
            {
                case 1:
                    Player.Instance.Damage(1f);
                    break;
                case 2:
                    float dmgTemp = Player.Instance.dmg / 2f;
                    Player.Instance.dmg -= dmgTemp;
                    yield return new WaitForSeconds(5);
                    Player.Instance.dmg += dmgTemp;
                    break;
                case 3:
                    float temp = Player.Instance.GetComponent<Movement>().moveSpeed / 4f;
                    Player.Instance.GetComponent<Movement>().moveSpeed -= temp;
                    yield return new WaitForSeconds(5);
                    Player.Instance.GetComponent<Movement>().moveSpeed += temp;
                    break;
                default:
                    break;
            }
        }
    }
}
