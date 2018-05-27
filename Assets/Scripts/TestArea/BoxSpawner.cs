using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxSpawner : MonoBehaviour
{
    public GameObject SpawnPrefab;
    public float SpawnDelay = 0.5f;

    [SerializeField]
    private GameObject _lastSpawn;
    
    [SerializeField]
    private Image _cooldownImage;

    private float _currentTimeToSpawn;

    public void RemoveLastSpawn()
    {
        if(_lastSpawn != null)
        {
            Destroy(_lastSpawn);
        }
    }

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

            _cooldownImage.fillAmount = 1 - _currentTimeToSpawn / SpawnDelay;

            if (_currentTimeToSpawn <= 0)
            {
                Spawn();
            }
        }
    }
}
