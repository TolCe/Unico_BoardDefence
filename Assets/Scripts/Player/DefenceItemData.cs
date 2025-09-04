using UnityEngine;

public class DefenceItemData
{
    [SerializeField] private float _damage;
    public float Damage { get { return _damage; } }

    [SerializeField] private int _range;
    public int Range { get { return _range; } }

    [SerializeField] private float _attackInterval;
    public float AttackInterval { get { return _attackInterval; } }

    [SerializeField] private Enums.DefenceItemAttackDir _attackDirection;
    public Enums.DefenceItemAttackDir AttackDirection { get { return _attackDirection; } }
}