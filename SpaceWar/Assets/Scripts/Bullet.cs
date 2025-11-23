using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    private Vector3 _bulletSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _bulletSpeed = Vector3.up * 6; 
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
