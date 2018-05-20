using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject SpawnPrefab;
    public float SpawnDelay = 0.5f;

    [SerializeField]
    private GameObject _lastSpawn;

    private float _currentTimeToSpawn;
    
    private void Spawn()
    {
        _lastSpawn = Instantiate(SpawnPrefab, transform.position + Vector3.up, transform.rotation);
        _currentTimeToSpawn = SpawnDelay;
    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        if (_lastSpawn == null)
        {
            _currentTimeToSpawn -= Time.deltaTime;

            if (_currentTimeToSpawn <= 0)
            {
                Spawn();
            }
        }
    }
}
