using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelFile;

    private void Start()
    {
        LevelData data = JsonUtility.FromJson<LevelData>(levelFile.text);

        GridController.Instance.CreateGrid(data.Rows, data.Columns);

        EnemySpawner.Instance.SpawnEnemies(data.EnemyDataList);

        DefenceItemsListing.Instance.ListUIElements(data.DefenceItemsDataList);
    }
}
