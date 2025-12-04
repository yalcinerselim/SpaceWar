using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private List<GameObject> enemyPrefabs;

    private float _duration;
    private float _nextSpawn;

    private void Start()
    {
        _nextSpawn = 5f;
        _duration = 2f;
    }

    private void Update()
    {
        if (!(Time.time > _nextSpawn)) return;
        SpawnEnemyShip(GetRandomSpawnPoint());
        _nextSpawn += _duration;
    }

    private void SpawnEnemyShip(Transform spawnPoint)
    {
        Instantiate(GetRandomEnemyPrefab(), spawnPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
    }

    private GameObject GetRandomEnemyPrefab()
    {
        var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        return enemyPrefab;
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}
