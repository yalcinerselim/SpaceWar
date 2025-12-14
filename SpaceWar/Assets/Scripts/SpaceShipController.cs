using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipController : MonoBehaviour, IDamageable
{
    private InputSystem_Actions _actions;

    private Rigidbody _rb;

    private Vector2 _speed;
    
    private Vector2 _moveDirection;
    
    private Vector3 _fireDirection;
    
    // SpaceShip objesine bağlı child Turbo objesinin referansı
    [SerializeField] private SpriteRenderer turbo;

    [SerializeField] private MachineGunsController machineGunsController;
    
    [SerializeField] private SpaceShipHealthController spaceShipHealthController;
    
    private Camera _mainCam;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _mainCam = Camera.main;
        _actions = new InputSystem_Actions();
        _actions.SpaceShip.Enable();
        
        _actions.SpaceShip.Sprint.performed += SetSpeed;
        _actions.SpaceShip.Sprint.canceled += SetSpeed;
        
        _actions.SpaceShip.Attack.performed += MachineGunAttackHandler;
        _actions.SpaceShip.Attack.canceled += MachineGunAttackHandler;
        
        turbo.enabled = false;

        _speed = new Vector2(3,3);
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
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = _mainCam.ScreenToWorldPoint(mouseScreenPosition);
        _fireDirection = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        
        machineGunsController.SetFireDirection(_fireDirection);
        
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
