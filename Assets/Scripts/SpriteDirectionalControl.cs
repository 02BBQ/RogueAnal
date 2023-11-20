using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirectionalControl : MonoBehaviour
{
    //[SerializeField] float backAngle = 65f;
    //[SerializeField] float sideAngle = 155f;
    [SerializeField] private Transform m_transform;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x,0f,Camera.main.transform.forward.z);
        Debug.DrawRay(Camera.main.transform.position, camForwardVector * 5f,Color.magenta) ;

        //float singledAngle = Vector3.SignedAngle(m_transform.forward,camForwardVector,Vector3.up);

        //Vector2 animationDirection = new Vector2(0f, -1f);

        //float angle = Mathf.Abs(singledAngle);

        //if (angle > backAngle)
        //{
        //    animationDirection
        //}
    }
}
