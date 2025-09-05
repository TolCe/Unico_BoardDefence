using System.Collections.Generic;
using UnityEngine;

public class DefenceItemsListing : SingletonMonoBehaviour<DefenceItemsListing>
{
    [SerializeField] private RectTransform _listingContainer;

    [SerializeField] private DefenceItemUIListElement _itemUIElementPrefab;

    private ObjectPool<DefenceItemUIListElement> _itemsPool;

    [SerializeField] private DefenceItemsDatabase _itemsDatabase;

    public void ListUIElements(List<LevelDefenceItemData> itemsDataList)
    {
        if (_itemsPool == null)
        {
            CreatePool();
        }

        for (int i = 0; i < itemsDataList.Count; i++)
        {
            DefenceItemUIListElement element = _itemsPool.Get();
            element.Initialize(itemsDataList[i]);
        }
    }

    private void CreatePool()
    {
        _itemsPool = new ObjectPool<DefenceItemUIListElement>(_itemUIElementPrefab, 10, _listingContainer);
    }

    public void ReturnToPool(DefenceItemUIListElement element)
    {
        _itemsPool.Return(element);
    }
}