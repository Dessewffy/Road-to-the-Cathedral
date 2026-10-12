using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _enemyCount = 5;

    public void SpawnEnemies()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            Instantiate(
                _enemyPrefab,
                _spawnPoint.position,
                _spawnPoint.rotation
            );
        }
    }


}