using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>, IPooler
{
    [SerializeField] private EnemyDatabase _enemyDatabase;

    [SerializeField] private Transform _enemyContainer;

    [SerializeField] private float _enemySpawnInterval;

    private Dictionary<int, ObjectPool<Enemy>> _enemyPoolDictionary;

    private Coroutine _enemySpawnCoroutine;

    private List<LevelEnemyData> _levelEnemyDataList;

    private List<Enemy> _spawnedEnemiesList = new List<Enemy>();

    private bool _spawnedAll;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        LevelLoader.Instance.OnReset += OnResetPool;
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
            CreatePool();
        }

        if (_spawnedEnemiesList == null)
        {
            _spawnedEnemiesList = new List<Enemy>();
        }

        _spawnedAll = false;

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

        _spawnedAll = true;
    }

    private Tile GetRandomTileFromFirstRow()
    {
        return GridController.Instance.GetRandomTileFromFirstRow();
    }

    public void OnResetPool()
    {
        if (_spawnedEnemiesList != null)
        {
            foreach (Enemy item in _spawnedEnemiesList)
            {
                item.Destroy();
            }

            _spawnedEnemiesList = new List<Enemy>();
        }
    }

    public void CreatePool()
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

        if (_spawnedEnemiesList.Count <= 0 && _spawnedAll)
        {
            GameManager.Instance.OnGameEnd(true);
        }
    }
}