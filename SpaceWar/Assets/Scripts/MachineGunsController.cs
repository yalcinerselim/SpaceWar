using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunsController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> machineGuns;
    
    [SerializeField] private GameObject bulletPrefab;

    public bool Attacking { get; set; }

    private float _nextFire;
    private float _fireRate;

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
            Instantiate(bulletPrefab, machineGunPosition);
        }
    }
}
