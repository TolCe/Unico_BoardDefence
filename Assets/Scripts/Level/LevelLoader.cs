using System;
using UnityEngine;

public class LevelLoader : SingletonMonoBehaviour<LevelLoader>
{
    [SerializeField] private LevelDatabase _levelDatabase;

    public Action OnReset;

    private const string LevelIndexKey = "LevelIndex";
    public int LevelIndex { get { return PlayerPrefs.GetInt(LevelIndexKey, 0); } private set { PlayerPrefs.SetInt(LevelIndexKey, value); } }

    private void Start()
    {
        ResetLevel();

        LoadLevel();
    }

    public void LoadLevel()
    {
        LevelData data = JsonUtility.FromJson<LevelData>(_levelDatabase.LevelJsonDataList[LevelIndex].text);

        EnemySpawner.Instance.SetEnemyData(data.EnemyDataList);

        GridController.Instance.CreateGrid(data.Rows, data.Columns);

        DefenceItemsUIListing.Instance.ListUIElements(data.DefenceItemsDataList);

        GameManager.Instance.OnLevelLoaded();
    }

    public void ResetLevel()
    {
        OnReset?.Invoke();
    }

    public void OnLevelRestart()
    {
        ResetLevel();

        LoadLevel();
    }

    public void OnNextLevel()
    {
        ResetLevel();

        LevelIndex++;

        if (LevelIndex >= _levelDatabase.LevelJsonDataList.Count)
        {
            LevelIndex = 0;
        }

        LoadLevel();
    }
}
