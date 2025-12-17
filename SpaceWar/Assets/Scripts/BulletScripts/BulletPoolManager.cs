using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletHolder;
    [SerializeField] private int poolSize = 100;

    // Havuzdaki "Kullanılabilir" mermiler
    private List<Bullet> _availableBullets;

    private void Awake()
    {
        // SINGLETON KURULUMU (Standart Unity Yöntemi)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Eğer sahnede 2 tane varsa ikincisini yok et
            return;
        }
        
        // Listeyi başlat
        _availableBullets = new List<Bullet>();
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void CreateBulletPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }
    
    private Bullet CreateNewBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletHolder);
        bullet.gameObject.SetActive(false);
        
        bullet.OnBulletInactive += ReturnBulletToPool; 
        
        _availableBullets.Add(bullet);
        return bullet;
    }

    private void ReturnBulletToPool(Bullet bullet)
    {
        if (!_availableBullets.Contains(bullet))
        {
            _availableBullets.Add(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (_availableBullets.Count > 0)
        {
            Bullet bullet = _availableBullets[_availableBullets.Count - 1];
            
            _availableBullets.RemoveAt(_availableBullets.Count - 1);
            
            return bullet;
        }
        
        // Eğer havuz bittiyse yeni yarat ve gönder (Dynamic Expansion)
        // Yeni yarattığımız mermiyi listeye eklemiyoruz, direkt gönderiyoruz.
        // O işi bitince zaten ReturnBulletToPool ile listeye girecek.
        Bullet newBullet = CreateNewBullet();
        _availableBullets.Remove(newBullet); // CreateNewBullet listeye ekliyordu, hemen geri çıkardık.
        return newBullet;
    }
}