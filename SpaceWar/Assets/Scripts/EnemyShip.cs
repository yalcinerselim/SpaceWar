using UnityEngine;
using UnityEngine.Serialization;

public class EnemyShip : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject spaceShip;
    
    [SerializeField] private MachineGunsController machineGunsController;

    private Rigidbody _rb;
    public int shipSpeed;
    private Vector3 _moveDirection = Vector3.down;
    
    private readonly float _visualRange = 10f;
    public int shipHealth;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        machineGunsController.SetBulletDirection(_moveDirection);
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

        if (Physics.Raycast(origin, direction, out var hitInfo, _visualRange))
        {
            if(hitInfo.collider.name == spaceShip.name)
            {
                MachineGunAttackHandler(true);
            }
            else
            {
                MachineGunAttackHandler(false);
            }
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

}
