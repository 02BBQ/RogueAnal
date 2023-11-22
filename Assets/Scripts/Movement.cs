using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 10f; // 이동 속도 조절 변수
    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;
    public float moveMultiple = 1;

    //[SerializeField] private AudioClip[] stepAudioClip;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //ez moving
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 direction = Vector3.ClampMagnitude(new Vector3(moveX, 0.0f, moveZ), 1f);// * Time.deltaTime;
        animator.SetFloat("Walking", direction.magnitude);
        //rb.velocity = (direction * moveSpeed * moveMultiple) ;
        rb.AddForce(direction*moveSpeed*moveMultiple*Time.deltaTime*500);
    }

    //public void OnFootStep(bool right)
    //{
    //    audioSource.PlayOneShot(stepAudioClip[right == true ? 0 : 1]);
    //}
}
