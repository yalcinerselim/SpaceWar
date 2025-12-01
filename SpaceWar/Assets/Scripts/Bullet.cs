using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    private Vector3 _bulletSpeed;
    public Vector3 bulletDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _bulletSpeed = bulletDirection * 10; 
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + _bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<SpaceShipController>())
        {
            Destroy(gameObject);
        }
    }
}
