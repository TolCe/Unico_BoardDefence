using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    [SerializeField] private EnemyDatabase _enemyDatabase;

    [SerializeField] private Transform _enemyContainer;

    [SerializeField] private float _enemySpawnInterval;

    private Dictionary<int, ObjectPool<Enemy>> _enemyPoolDictionary;

    private Coroutine _enemySpawnCoroutine;

    private List<LevelEnemyData> _levelEnemyDataList;

    private List<Enemy> _spawnedEnemiesList;

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    public void SetEnemyData(List<LevelEnemyData> levelEnemyDataList)
    {
        _levelEnemyDataList = levelEnemyDataList;
    }

    private void OnGameStateChange()
    {
        if (GameManager.Instance.GameState == Enums.GameState.Playing)
        {
            SpawnEnemies(_levelEnemyDataList);
        }
        else
        {
            if (_enemySpawnCoroutine != null)
            {
                StopCoroutine(_enemySpawnCoroutine);
                _enemySpawnCoroutine = null;
            }
        }
    }

    public void SpawnEnemies(List<LevelEnemyData> enemyDataList)
    {
        if (_enemyPoolDictionary == null)
        {
            CreatePools();
        }

        if (_spawnedEnemiesList == null)
        {
            _spawnedEnemiesList = new List<Enemy>();
        }

        _enemySpawnCoroutine = StartCoroutine(SpawnCoroutine(enemyDataList));
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

                _spawnedEnemiesList.Add(enemy);

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

    public void OnEnemyKilled(Enemy enemy)
    {
        _spawnedEnemiesList.Remove(enemy);

        _enemyPoolDictionary[enemy.EnemyData.Level].Return(enemy);
    }
}