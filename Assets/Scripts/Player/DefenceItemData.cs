using UnityEngine;

[CreateAssetMenu(fileName = "DefenceItem_", menuName = "Defence Items/Item Data")]
public class DefenceItemData : ScriptableObject
{
    [SerializeField] private int _level;
    public int Level { get { return _level; } }

    [SerializeField] private DefenceItem _prefab;
    public DefenceItem Prefab { get { return _prefab; } }

    [SerializeField] private float _damage;
    public float Damage { get { return _damage; } }

    [SerializeField] private int _range;
    public int Range { get { return _range; } }

    [SerializeField] private float _attackInterval;
    public float AttackInterval { get { return _attackInterval; } }

    [SerializeField] private Enums.DefenceItemAttackDir _attackDirection;
    public Enums.DefenceItemAttackDir AttackDirection { get { return _attackDirection; } }
}