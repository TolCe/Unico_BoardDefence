using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelFile;

    private void Start()
    {
        LevelData data = JsonUtility.FromJson<LevelData>(levelFile.text);

        EnemySpawner.Instance.SetEnemyData(data.EnemyDataList);

        GridController.Instance.CreateGrid(data.Rows, data.Columns);

        DefenceItemsListing.Instance.ListUIElements(data.DefenceItemsDataList);
    }
}
