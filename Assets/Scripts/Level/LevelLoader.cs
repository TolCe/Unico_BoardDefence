using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelFile;
    public GameObject fullTilePrefab;
    public GameObject enemyPrefab;

    void Start()
    {
        LevelData data = JsonUtility.FromJson<LevelData>(levelFile.text);

        for (int r = 0; r < data.Rows; r++)
        {
            for (int c = 0; c < data.Columns; c++)
            {
                Vector3 pos = new Vector3(c, 0, -r);
                Instantiate(fullTilePrefab, pos, Quaternion.identity);
            }
        }

        // Spawn enemies based on list
        foreach (LevelEnemyData enemyData in data.EnemyDataList)
        {
            for (int i = 0; i < enemyData.Count; i++)
            {
                Vector3 pos = new Vector3(Random.Range(0, data.Columns), 0, -Random.Range(0, data.Rows));
                GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

                EnemySpawner.Instance.SpawnEnemy(enemyData);
            }
        }
    }
}
