using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance { get; private set; }
    
    [SerializeField] private List<EnemyShip> enemyShipPrefabs;

    private Dictionary<string, Queue<EnemyShip>> _enemyShipPool;
    

    [SerializeField] private int poolSizePerEnemy = 100;

    private void Awake()
    {
        // SINGLETON KURULUMU (Standart Unity Yöntemi)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _enemyShipPool = new Dictionary<string, Queue<EnemyShip>>();
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var prefab in enemyShipPrefabs)
        {
            Queue<EnemyShip> newQueue = new Queue<EnemyShip>();
            
            string key = prefab.name;

            for (int i = 0; i < poolSizePerEnemy; i++)
            {
                EnemyShip enemy = CreateNewEnemy(prefab);
                newQueue.Enqueue(enemy);
            }
            
            // Sözlüğe ekle
            _enemyShipPool.Add(key, newQueue);
        }
    }

    private EnemyShip CreateNewEnemy(EnemyShip prefab)
    {
        EnemyShip enemy = Instantiate(prefab, transform);
        enemy.name = prefab.name; // İsim prefab ile aynı kalmalı ki Dictionary'den bulabilelim
        enemy.gameObject.SetActive(false);
        
        // Düşman öldüğünde havuza dönmesi için abone oluyoruz
        enemy.OnKilled += ReturnEnemyToPool;
        
        return enemy;
    }

    private void ReturnEnemyToPool(EnemyShip destroyedShip)
    {
        // Düşmanın ismine (key) göre doğru kuyruğu bul ve ekle
        if (_enemyShipPool.ContainsKey(destroyedShip.name))
        {
            _enemyShipPool[destroyedShip.name].Enqueue(destroyedShip);
        }
    }
    
    public EnemyShip GetEnemy(string enemyType)
    {
        // İstenen düşman tipi havuzda var mı?
        if (_enemyShipPool.ContainsKey(enemyType) && _enemyShipPool[enemyType].Count > 0)
        {
            EnemyShip enemy = _enemyShipPool[enemyType].Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            // Havuz bitmişse ne yapalım? 
            // 1. Seçenek: Yeni üret (Dynamic Expansion) - Aşağıdaki kodu kullanabilirsin
            // 2. Seçenek: null dön (Spawnlama)
            
            // Ben şimdilik prefab listesinden o isme ait prefabı bulup üretiyorum
            EnemyShip prefabToSpawn = enemyShipPrefabs.Find(x => x.name == enemyType);
            if(prefabToSpawn is not null)
            {
                return CreateNewEnemy(prefabToSpawn); // Yeni üretip gönderiyoruz, ölünce havuza girecek.
            }
            
            return null;
        }
    }
}
