using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipController : MonoBehaviour, IDamageable
{
    private InputSystem_Actions _actions;

    private Rigidbody _rb;

    private Vector2 _speed;
    
    private Vector2 _moveDirection;
    
    private readonly Vector2 _fireDirection = Vector3.up;
    
    // SpaceShip objesine bağlı child Turbo objesinin referansı
    [SerializeField] private SpriteRenderer turbo;

    [SerializeField] private MachineGunsController machineGunsController;
    
    [SerializeField] private SpaceShipHealthController spaceShipHealthController;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _actions = new InputSystem_Actions();
        _actions.SpaceShip.Enable();
        
        _actions.SpaceShip.Sprint.performed += SetSpeed;
        _actions.SpaceShip.Sprint.canceled += SetSpeed;
        
        _actions.SpaceShip.Attack.performed += MachineGunAttackHandler;
        _actions.SpaceShip.Attack.canceled += MachineGunAttackHandler;
        
        turbo.enabled = false;

        _speed = new Vector2(2,2);
        machineGunsController.SetBulletDirection(_fireDirection);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _moveDirection = _actions.SpaceShip.Move.ReadValue<Vector2>() * _speed;
        _rb.MovePosition(_rb.position + new Vector3(_moveDirection.x, _moveDirection.y, 0) * Time.deltaTime);
    }

    private void SetSpeed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
        {
            _speed *= 2;
            turbo.enabled = true;
        }
        else
        {
            _speed /= 2;
            turbo.enabled = false;
        }
    }

    private void MachineGunAttackHandler(InputAction.CallbackContext ctx)
    {
        machineGunsController.Attacking = ctx.ReadValueAsButton();
    }

    public void TakeDamage(int damageAmount)
    {
        damageAmount -= 10;
        spaceShipHealthController.TakeDamageHandler(damageAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(1000);
            TakeDamage(30);
        }
    }
}
