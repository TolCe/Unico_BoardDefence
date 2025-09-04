using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int _level;
    public int Level { get { return _level; } }

    [SerializeField] private Enemy _prefab;
    public Enemy Prefab { get { return _prefab; } }

    [SerializeField] private float _health;
    public float Health { get { return _health; } }

    [Header("Speed unit: Blocks/second")]
    [SerializeField] private float _speed;
    public float Speed { get { return _speed; } }
}