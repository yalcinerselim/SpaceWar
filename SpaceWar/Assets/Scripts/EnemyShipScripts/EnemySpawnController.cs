using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    [Header("Settings")]
    // Spawn süreleri ScriptableObject'ten gelirse daha iyi olur ama şimdilik Inspector'da kalsın
    [SerializeField] private float spawnInterval = 2f; 
    
    [Header("References")]
    [SerializeField] private List<Transform> spawnPoints;
    
    // Sadece hangi tiplerin spawnlanacağını bilmesi yeterli (İsim listesi veya Prefab referansı)
    [SerializeField] private List<EnemyShip> enemyTypes; 

    private float _nextSpawnTime;

    private void Start()
    {
        _nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time > _nextSpawnTime)
        {
            SpawnEnemy();
            _nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // 1. Rastgele Konum Seç
        Transform point = GetRandomSpawnPoint();
        
        // 2. Rastgele Düşman Tipi Seç
        EnemyShip randomEnemyPrefab = GetRandomEnemyType();
        
        // 3. Pool Manager'dan bu düşmanı iste (Prefab ismini gönderiyoruz)
        EnemyShip enemy = EnemyPoolManager.Instance.GetEnemy(randomEnemyPrefab.name);

        if (enemy is not null)
        {
            // Pozisyonu ve Rotasyonu ayarla
            enemy.transform.position = point.position;
            
            // Senin 180 derece döndürme mantığın:
            enemy.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private EnemyShip GetRandomEnemyType()
    {
        return enemyTypes[Random.Range(0, enemyTypes.Count)];
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}