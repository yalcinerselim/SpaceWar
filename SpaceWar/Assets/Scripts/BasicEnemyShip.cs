using System;
using UnityEngine;

public class BasicEnemyShip : UnarmoredShip
{
    private Rigidbody _rb;
    private Vector3 _shipSpeed;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _shipSpeed = Vector3.down * 2;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + _shipSpeed * Time.deltaTime);
    }
}
