using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private LevelData _levelData;

    private Vector2 _enemyScroll;
    private Vector2 _defenceItemScroll;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnEnable()
    {
        if (_levelData == null)
        {
            _levelData = new LevelData(8, 4);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Grid Size", EditorStyles.boldLabel);

        int newRows = EditorGUILayout.IntField("Rows", _levelData.Rows);
        int newCols = EditorGUILayout.IntField("Columns", _levelData.Columns);

        EditorGUILayout.Space();

        DefenceItemSettings();

        EditorGUILayout.Space();

        EnemySettings();

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Level"))
        {
            string path = EditorUtility.SaveFilePanel("Save Level", "Assets/Data/Levels", "LevelData_", "json");
            if (!string.IsNullOrEmpty(path))
            {
                string json = JsonUtility.ToJson(_levelData, true);
                System.IO.File.WriteAllText(path, json);
                AssetDatabase.Refresh();
            }
        }
        else if (GUILayout.Button("Load Level"))
        {
            string path = EditorUtility.OpenFilePanel("Load Level", "Assets/Data/Levels", "json");
            if (!string.IsNullOrEmpty(path))
            {
                string json = System.IO.File.ReadAllText(path);
                LevelData loadedData = JsonUtility.FromJson<LevelData>(json);
                if (loadedData != null)
                {
                    _levelData = loadedData;
                }
                else
                {
                    Debug.LogError("Failed to load level data.");
                }
            }
        }
    }

    private void DefenceItemSettings()
    {
        EditorGUILayout.LabelField("Defence Items", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Defence Item"))
        {
            _levelData.DefenceItemsDataList.Add(new LevelDefenceItemData { Level = _levelData.DefenceItemsDataList.Count, Count = 1 });
        }

        _defenceItemScroll = EditorGUILayout.BeginScrollView(_defenceItemScroll, GUILayout.Height(120));

        for (int i = 0; i < _levelData.DefenceItemsDataList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            _levelData.DefenceItemsDataList[i] = new LevelDefenceItemData
            {
                Level = EditorGUILayout.IntField("Level", _levelData.DefenceItemsDataList[i].Level),
                Count = EditorGUILayout.IntField("Count", _levelData.DefenceItemsDataList[i].Count)
            };

            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                _levelData.DefenceItemsDataList.RemoveAt(i);

                GUILayout.EndHorizontal();

                break;
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void EnemySettings()
    {
        EditorGUILayout.LabelField("Enemies", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Enemy"))
        {
            _levelData.EnemyDataList.Add(new LevelEnemyData { Level = _levelData.EnemyDataList.Count, Count = 1 });
        }

        _enemyScroll = EditorGUILayout.BeginScrollView(_enemyScroll, GUILayout.Height(120));

        for (int i = 0; i < _levelData.EnemyDataList.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            _levelData.EnemyDataList[i] = new LevelEnemyData
            {
                Level = EditorGUILayout.IntField("Level", _levelData.EnemyDataList[i].Level),
                Count = EditorGUILayout.IntField("Count", _levelData.EnemyDataList[i].Count)
            };

            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                _levelData.EnemyDataList.RemoveAt(i);

                GUILayout.EndHorizontal();

                break;
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }
}
