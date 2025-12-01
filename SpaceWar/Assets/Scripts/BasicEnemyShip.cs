using UnityEngine;

public class BasicEnemyShip : UnarmoredShip
{
    [SerializeField] private GameObject spaceShip;
    
    [SerializeField] private MachineGunsController machineGunsController;
    
    private Rigidbody _rb;
    private Vector3 _shipSpeed;
    private readonly Vector3 _fireDirection = Vector3.down;
    
    private readonly float _visualRange = 10f;
    
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _shipSpeed = _fireDirection * 2;
        machineGunsController.SetBulletDirection(_fireDirection);
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
        _rb.MovePosition(_rb.position + _shipSpeed * Time.deltaTime);
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
            
            Debug.DrawRay(origin, direction * hitInfo.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(origin, direction * _visualRange, Color.green);
        }
    }
    
    private void MachineGunAttackHandler(bool value)
    {
        machineGunsController.ChangeAttackingSituation(value);
    }
}
