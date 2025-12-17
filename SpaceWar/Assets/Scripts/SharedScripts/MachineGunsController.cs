using System.Collections.Generic;
using UnityEngine;

public class MachineGunsController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<Transform> machineGuns;

    public bool Attacking { get; set; }

    private float _fireRate; // Değeri dışarıdan alacak
    private float _nextFireTime;
    private Vector3 _fireDirection;
    
    // Layer Caching
    private int _playerLayer;
    private int _enemyLayer;
    private int _playerBulletLayer;
    private int _enemyBulletLayer;
    private int _myBulletLayer;

    private void Awake()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _playerBulletLayer = LayerMask.NameToLayer("PlayerBullet");
        _enemyBulletLayer = LayerMask.NameToLayer("EnemyBullet");
    }

    private void Start()
    {
        Attacking = false;
        
        if (gameObject.layer == _playerLayer)
        {
            _myBulletLayer = _playerBulletLayer;
        }
        else if (gameObject.layer == _enemyLayer)
        {
            _myBulletLayer = _enemyBulletLayer;
            _fireDirection = Vector3.down; 
        }
    }
    
    public void Configure(float newFireRate)
    {
        _fireRate = newFireRate;
    }

    private void Update()
    {
        if (Attacking && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + _fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        foreach (var gunPos in machineGuns)
        {
            Bullet bullet = BulletPoolManager.Instance.GetBullet();

            if (bullet != null)
            {
                // 2. Mermiyi namlunun ucuna ışınla
                bullet.transform.position = gunPos.position;
                bullet.transform.rotation = Quaternion.identity;
                
                // 3. Mermiyi aktif et (Havuz kapalı vermiş olabilir)
                bullet.gameObject.SetActive(true);

                // 4. Ayarları yap (Eski kodun aynısı)
                bullet.SetBulletLayerMask(_myBulletLayer);
                bullet.SetBulletDirection(_fireDirection);
            }
        }
    }
    
    public void SetFireDirection(Vector3 targetPosition)
    {
        _fireDirection = (targetPosition - transform.position).normalized;
    }
}