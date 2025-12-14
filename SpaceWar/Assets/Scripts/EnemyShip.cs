using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyShip : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject spaceShip;
    
    [SerializeField] private MachineGunsController machineGunsController;

    private Rigidbody _rb;
    public int shipSpeed;
    private Vector3 _moveDirection = Vector3.down;
    
    private readonly float _visualRange = 30f;
    public int shipHealth;
    
    public LayerMask hitLayers;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveDirection *= shipSpeed;
    }

    private void Update()
    {
        Radar();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + _moveDirection * Time.deltaTime);
    }

    private void Radar()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.up;
        
        if (Physics.Raycast(transform.position, direction, out var hitInfo, _visualRange, hitLayers))
        {
            MachineGunAttackHandler(true);
            Debug.DrawRay(origin, direction * hitInfo.distance, Color.red);
        }
        else
        {
            // Bir şeye çarpmıyorsa, ışını maksimum mesafeye kadar yeşil çiz
            Debug.DrawRay(origin, direction * _visualRange, Color.green);
            MachineGunAttackHandler(false);
        }
    }
    
    private void MachineGunAttackHandler(bool value)
    {
        machineGunsController.ChangeAttackingSituation(value);
    }
    
    public virtual void TakeDamage(int damageAmount)
    {
        shipHealth -= damageAmount;
        if (shipHealth <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LethalBoundary>())
        {
            Die();
        }
    }
}
