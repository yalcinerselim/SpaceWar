using System;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public UnityAction<Bullet> OnBulletInactive;
    private Rigidbody _rb;
    
    [SerializeField] private float speed = 10f; // Hızı inspector'dan değiştirebilmek için değişkene aldım
    private int _damage = 20;

    private Vector3 _bulletSpeed;
    private Vector3 _bulletDirection; // public olmasına gerek yok, private yaptım

    private void Awake()
    {
        // Component alımlarını Awake'te yapmak daha güvenlidir.
        // Obje her açılıp kapandığında bu referans kaybolmaz.
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Eğer mermi aktifse hareket etmeye devam et
        Move();
    }

    private void Move()
    {
        // _bulletSpeed değeri SetBulletDirection içinde güncellendiği için
        // burada her zaman doğru yöne gider.
        _rb.MovePosition(_rb.position + _bulletSpeed * Time.fixedDeltaTime);
    }

    public void SetBulletDirection(Vector3 direction)
    {
        _bulletDirection = direction;
        
        // Görsel olarak mermiyi o yöne çevir
        transform.up = direction;
        
        // KRİTİK DÜZELTME:
        // Hız vektörünü, yön her değiştiğinde BURADA yeniden hesaplıyoruz.
        // Start içinde değil!
        _bulletSpeed = _bulletDirection * speed; 
    }

    public void SetBulletLayerMask(int layerIndex) 
    {
        // LayerMask struct yerine direkt int index almak daha performanslı ve kolaydır
        gameObject.layer = layerIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObj = other.GetComponent<IDamageable>();
        
        // null check için kısa kullanım (C# sugar)
        damageableObj?.TakeDamage(_damage);
        
        // Mermiyi yok etme, pasif yap (Havuza dönsün)
        Deactivate();
    }
    
    // Mermiyi manuel kapatmak istersen (örn: ekrandan çıkınca) bu metodu çağırabilirsin
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Obje kapandığı an Manager'a haber veriyoruz
        OnBulletInactive?.Invoke(this);
        
        // Tavsiye: Mermi kapandığında hızını sıfırlamak iyi bir alışkanlıktır
        // böylece bir sonraki açılışta bir anlık eski yöne kayma (ghosting) olmaz.
        _bulletSpeed = Vector3.zero;
    }
}