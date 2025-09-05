using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Level/Level Database")]
public class LevelDatabase : ScriptableObject
{
    [SerializeField] private List<TextAsset> _levelJsonDataList;
    public List<TextAsset> LevelJsonDataList { get { return _levelJsonDataList; } }
}