using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelFile;

    private void Start()
    {
        LevelData data = JsonUtility.FromJson<LevelData>(levelFile.text);

        GridController.Instance.CreateGrid(data.Rows, data.Columns);

        foreach (LevelEnemyData enemyData in data.EnemyDataList)
        {
            for (int i = 0; i < enemyData.Count; i++)
            {
                Vector2 pos = new Vector3(Random.Range(0, data.Columns), -Random.Range(0, data.Rows));

                EnemySpawner.Instance.SpawnEnemy(enemyData, pos);
            }
        }
    }
}
