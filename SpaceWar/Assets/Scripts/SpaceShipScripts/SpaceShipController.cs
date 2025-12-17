using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipController : DestructibleObject
{
    [Header("Dependencies")]
    [SerializeField] private ShipStatsSO stats; // Model
    [SerializeField] private SpaceShipVisuals visuals; // View
    [SerializeField] private MachineGunsController machineGunsController;

    private InputSystem_Actions _actions;
    private Rigidbody _rb;
    private Camera _mainCam;
    private Vector2 _moveInput;
    private bool _isSprinting;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
        
        // Input Actions'ı burada new'lemek yerine, genelde global bir InputManager'dan almak daha iyidir
        // ama şimdilik bu yapını bozmuyorum.
        _actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        InitializeHealth(stats.Health, stats.ArmorValue);
        _actions.SpaceShip.Enable();
        // Event abonelikleri
        _actions.SpaceShip.Sprint.performed += OnSprintStarted;
        _actions.SpaceShip.Sprint.canceled += OnSprintEnded;
        _actions.SpaceShip.Attack.performed += OnAttackStarted;
        _actions.SpaceShip.Attack.canceled += OnAttackEnded;
        
        machineGunsController.Configure(stats.FireRate);
    }

    private void OnDisable()
    {
        // Event aboneliklerini iptal etmek (Memory Leak önlemek için şart)
        _actions.SpaceShip.Sprint.performed -= OnSprintStarted;
        _actions.SpaceShip.Sprint.canceled -= OnSprintEnded;
        _actions.SpaceShip.Attack.performed -= OnAttackStarted;
        _actions.SpaceShip.Attack.canceled -= OnAttackEnded;
        _actions.SpaceShip.Disable();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Input oku
        _moveInput = _actions.SpaceShip.Move.ReadValue<Vector2>();

        // Hız hesapla (Model'den gelen veriyi kullan)
        float currentSpeed = _isSprinting ? stats.BaseSpeed * stats.SprintMultiplier : stats.BaseSpeed;

        // Fiziği uygula
        Vector3 displacement = new Vector3(_moveInput.x, _moveInput.y, 0) * (currentSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + displacement);
    }

    #region Input Handlers
    
    private void OnSprintStarted(InputAction.CallbackContext ctx)
    {
        _isSprinting = true;
        visuals.SetTurboVisual(true); // View'a emir ver
    }

    private void OnSprintEnded(InputAction.CallbackContext ctx)
    {
        _isSprinting = false;
        visuals.SetTurboVisual(false); // View'a emir ver
    }

    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        UpdateFireDirection(); // Ateş etmeden önce yönü güncelle
        machineGunsController.Attacking = true;
    }

    private void OnAttackEnded(InputAction.CallbackContext ctx)
    {
        machineGunsController.Attacking = false;
    }

    #endregion

    private void UpdateFireDirection()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        
        // ÖNEMLİ DÜZELTME: Kameranın oyun düzlemine olan uzaklığını Z olarak veriyoruz.
        // Kameranın z=-10'da, geminin z=0'da olduğunu varsayıyorum.
        mouseScreenPosition.z = -_mainCam.transform.position.z; 
        
        Vector3 mouseWorldPosition = _mainCam.ScreenToWorldPoint(mouseScreenPosition);
        
        // Z eksenini 0'a sabitliyoruz (2D oynanış için)
        Vector3 targetPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        
        machineGunsController.SetFireDirection(targetPosition);
    }

    protected override void Die()
    {
        Debug.Log("Space Ship Destroyed");
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Model'den gelen çarpışma hasarı verilerini kullan
            damageable.TakeDamage(stats.CollisionEnemyDamage);
        }
    }
}