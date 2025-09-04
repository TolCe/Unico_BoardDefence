using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Enemy/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private List<EnemyData> _enemyDataList;
    public List<EnemyData> EnemyDataList { get { return _enemyDataList; } }
}