using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private GameObject basicEnemyShipPrefab;

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
        Instantiate(basicEnemyShipPrefab, spawnPoint.transform.position, Quaternion.Euler(Vector3.zero));
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}
