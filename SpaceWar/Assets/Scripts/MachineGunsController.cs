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
        _fireRate = 0.5f;
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
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        foreach (var machineGunPosition in machineGuns)
        {
            var bullet = Instantiate(bulletPrefab, machineGunPosition);
            if (gameObject.layer == playerLayer)
            {
                bullet.SetBulletLayerMask(LayerMask.NameToLayer("PlayerBullet"));
            }
            else if (gameObject.layer == enemyLayer)
            {
                bullet.SetBulletLayerMask(LayerMask.NameToLayer("EnemyBullet"));
                _fireDirection = Vector3.down;
            }
            bullet.SetBulletDirection(_fireDirection);
        }
    }
    
    public void ChangeAttackingSituation(bool attacking)
    {
        Attacking = attacking;
    }

    public void SetFireDirection(Vector3 direction)
    {
        _fireDirection = (direction - transform.position).normalized;
    }
}
