using System.Collections.Generic;
using UnityEngine;

public class DefenceItemsUIListing : SingletonMonoBehaviour<DefenceItemsUIListing>, IPooler
{
    [SerializeField] private RectTransform _listingContainer;

    [SerializeField] private DefenceItemUIListElement _itemUIElementPrefab;

    private ObjectPool<DefenceItemUIListElement> _itemsPool;

    [SerializeField] private DefenceItemsDatabase _itemsDatabase;

    protected override void Awake()
    {
        base.Awake();

        LevelLoader.Instance.OnReset += OnResetPool;
    }

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

    public void OnResetPool()
    {
        if (_itemsPool != null)
        {
            _itemsPool.ReturnAll();
        }
    }

    public void CreatePool()
    {
        _itemsPool = new ObjectPool<DefenceItemUIListElement>(_itemUIElementPrefab, 10, _listingContainer);
    }

    public void ReturnToPool(DefenceItemUIListElement element)
    {
        _itemsPool.Return(element);
    }
}