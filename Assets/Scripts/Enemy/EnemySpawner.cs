using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    [SerializeField] private EnemyDatabase _enemyDatabase;

    [SerializeField] private Transform _enemyContainer;

    [SerializeField] private float _enemySpawnInterval;

    private Dictionary<int, ObjectPool<Enemy>> _enemyPoolDictionary;

    public void SpawnEnemies(List<LevelEnemyData> enemyDataList)
    {
        if (_enemyPoolDictionary == null)
        {
            CreatePools();
        }

        StartCoroutine(SpawnCoroutine(enemyDataList));
    }

    private IEnumerator SpawnCoroutine(List<LevelEnemyData> enemyDataList)
    {
        foreach (LevelEnemyData enemyData in enemyDataList)
        {
            EnemyData selectedEnemyData = _enemyDatabase.EnemyDataList.Find(x => x.Level == enemyData.Level);

            for (int i = 0; i < enemyData.Count; i++)
            {
                Enemy enemy = _enemyPoolDictionary[selectedEnemyData.Level].Get();
                enemy.SpawnEnemy(selectedEnemyData, GetRandomTileFromFirstRow());

                yield return new WaitForSeconds(_enemySpawnInterval);
            }
        }
    }

    private Tile GetRandomTileFromFirstRow()
    {
        return GridController.Instance.GetRandomTileFromFirstRow();
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