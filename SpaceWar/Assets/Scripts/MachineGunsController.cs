using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunsController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> machineGuns;
    
    [SerializeField] private Bullet bulletPrefab;

    public bool Attacking { get; set; }

    private float _nextFire;
    private float _fireRate;
    private Vector3 _fireDirection;

    private void Start()
    {
        Attacking = false;
        _fireRate = 0.2f;
        _nextFire = 0.0f;
    }

    private void Update()
    {
        if (Attacking && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        foreach (var machineGunPosition in machineGuns)
        {
            var bullet = Instantiate(bulletPrefab, machineGunPosition);
            bullet.bulletDirection = _fireDirection;
        }
    }
    
    public void ChangeAttackingSituation(bool attacking)
    {
        Attacking = attacking;
    }

    public void SetBulletDirection(Vector3 direction)
    {
        _fireDirection = direction;
    }
}
