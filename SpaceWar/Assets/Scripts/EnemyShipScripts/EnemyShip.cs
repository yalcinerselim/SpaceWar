using UnityEngine;
using UnityEngine.Events;

public class EnemyShip : MonoBehaviour, IDamageable
{
    // Havuz yöneticisinin dinleyeceği event
    public UnityAction<EnemyShip> OnEnemyKilled;

    [Header("Components")]
    [SerializeField] private MachineGunsController machineGunsController;
    [SerializeField] private EnemyShipStatsSO enemyShipStatsSo;
    

    private Rigidbody _rb;
    private int _currentHealth;
    
    // Raycast için ayarlar
    private readonly float _visualRange = 20f;
    [SerializeField] private LayerMask detectionLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // KRİTİK BÖLÜM: Object Pooling için Resetleme Alanı
    private void OnEnable()
    {
        // 1. Canı Fulle (ScriptableObject'ten al)
        _currentHealth = enemyShipStatsSo.Health;
        
        // 2. Saldırıyı durdur (Önceki hayattan kalma bir emir varsa iptal et)
        machineGunsController.Attacking = false;
        
        // 3. Fiziksel hızları sıfırla (Eğer patladığında fırladıysa, yeni doğuşta sabit dursun)
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        
        // SİLAH AYARI:
        // Düşman istatistik dosyasından gelen hızı silaha bildiriyoruz.
        // Örn: Düşman için FireRate 1.5f olabilir (daha yavaş)
        machineGunsController.Configure(enemyShipStatsSo.FireRate);
    }

    private void Update()
    {
        // Radar her karede çalışmalı
        HandleRadar();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Hızı her karede hesaplıyoruz ki SO'dan hızı değiştirirsen oyun oynarken de değişsin.
        // Vector3.down, düşmanın aşağı gideceğini varsayar.
        // Eğer gemi 180 derece dönükse "transform.up" da kullanabilirsin (aşağı bakıyor olur).
        Vector3 movement = Vector3.down * (enemyShipStatsSo.BaseSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + movement);
    }

    private void HandleRadar()
    {
        // Işın başlangıcı ve yönü
        Vector3 origin = transform.position;
        
        // DİKKAT: Eğer gemi görsel olarak 180 derece dönükse, 
        // transform.up AŞAĞIYI (oyuncuya doğru) gösterir. Bu doğru.
        Vector3 direction = transform.up; 
        
        // Physics.Raycast bool döner, if içine alabiliriz
        bool hitSomething = Physics.Raycast(origin, direction, out var hitInfo, _visualRange, detectionLayer);
        
        if (hitSomething)
        {
            // Oyuncuyu gördü, ateş serbest
            machineGunsController.Attacking = true;
            
            // Sadece Editörde çalışır, build alınca yok olur (Performans dostu)
            #if UNITY_EDITOR
            Debug.DrawRay(origin, direction * hitInfo.distance, Color.red);
            #endif
        }
        else
        {
            // Kimseyi görmüyor, ateşi kes
            machineGunsController.Attacking = false;
            
            #if UNITY_EDITOR
            Debug.DrawRay(origin, direction * _visualRange, Color.green);
            #endif
        }
    }

    // IDamageable implementation
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Beni kim dinliyorsa (PoolManager) ona haber ver
        OnEnemyKilled?.Invoke(this);
        
        // Asla Destroy yok! Kendini kapat.
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Duvara veya "Öldüren Sınıra" çarpınca
        if (other.GetComponent<LethalBoundary>())
        {
            Die();
        }
        // Oyuncuya çarparsa (Player scriptinde IDamageable varsa)
        // Bu kısım Player scriptinde yönetilebilir veya burada eklenebilir.
    }
}