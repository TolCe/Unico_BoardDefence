using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    [SerializeField] private EnemyDatabase _enemyDatabase;

    [SerializeField] private Transform _enemyContainer;

    [SerializeField] private float _enemySpawnInterval;

    private Dictionary<int, ObjectPool<Enemy>> _enemyPoolDictionary;

    public void SpawnEnemy(LevelEnemyData enemyData, Vector2 coordinates)
    {
        if (_enemyDatabase == null)
        {
            CreatePools();
        }

        EnemyData selectedEnemyData = _enemyDatabase.EnemyDataList.Find(x => x.Level == enemyData.Level);

        for (int i = 0; i < enemyData.Count; i++)
        {

        }
    }

    private void CreatePools()
    {
        _enemyPoolDictionary = new Dictionary<int, ObjectPool<Enemy>>();

        foreach (EnemyData data in _enemyDatabase.EnemyDataList)
        {
            ObjectPool<Enemy> pool = new ObjectPool<Enemy>(data.Prefab, 3, _enemyContainer);

            if (_enemyPoolDictionary.ContainsKey(data.Level))
            {
                Debug.LogWarning("There are more than one enemy data with same level!");

                continue;
            }

            _enemyPoolDictionary.Add(data.Level, pool);
        }
    }
}