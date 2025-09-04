using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private EnemyDatabase _enemyDatabase;

    public void SpawnEnemy(LevelEnemyData enemyData, Vector2 coordinates)
    {

    }
}