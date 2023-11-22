using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transgender : MonoBehaviour
{

    [SerializeField] private Material material;



    public float maskAmount = 0f;
    public float targetValue = -.1f;

    private void Update()
    {
        float maskAmountChange = targetValue > maskAmount ? +.1f : -.1f;
        maskAmount += maskAmountChange * Time.deltaTime * 25f;
        maskAmount = Mathf.Clamp(maskAmount, -.1f, 1f);

        material.SetFloat("_MaskAmount", maskAmount);
    }

    public void WhiteWashing()
    {
        targetValue = -.1f;
    }
    public void BlackWashing()
    {
        targetValue = 1f;

    }


}