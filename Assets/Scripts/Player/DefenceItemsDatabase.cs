using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenceItemsDatabase", menuName = "Defence Items/Database")]
public class DefenceItemsDatabase : ScriptableObject
{
    [SerializeField] private List<DefenceItemData> _itemDataList;
    public List<DefenceItemData> ItemDataList { get { return _itemDataList; } }
}