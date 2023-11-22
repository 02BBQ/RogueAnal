using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("MOVEMENT")]
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 25;

    private void OnEnable()
    {
        _target = Player.Instance.transform;
    }

    void FixedUpdate()
    {
        try
        {
            var heading = _target.position - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));

            _rb.velocity = transform.forward * _speed;
        }
        catch
        {
            print("Can't Find Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        ////if (collision.transform.TryGetComponent<IExplode>(out var ex)) ex.Explode();

        //Destroy(gameObject);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Money += 1;
            if (_explosionPrefab) PoolManager.Get(_explosionPrefab, transform.position, Quaternion.identity);
            CinemachineShake.Instance.ShakeCamera(10f, .1f);
            PoolManager.Release(gameObject);
        }
    }
}
