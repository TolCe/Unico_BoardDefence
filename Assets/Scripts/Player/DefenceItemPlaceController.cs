using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefenceItemPlaceController : SingletonMonoBehaviour<DefenceItemPlaceController>, IPooler
{
    private Dictionary<int, ObjectPool<DefenceItem>> _itemsPoolDictionary;

    [SerializeField] private Transform _itemsContainer;

    [SerializeField] private DefenceItemsDatabase _itemsDatabase;

    private List<DefenceItem> _spawnedDefenceItemsList;

    protected override void Awake()
    {
        base.Awake();

        LevelLoader.Instance.OnReset += OnResetPool;
    }

    public void OnSelectedForPlacing(DefenceItemUIListElement uiItem, LevelDefenceItemData data)
    {
        if (_itemsPoolDictionary == null)
        {
            CreatePool();
        }
        if (_spawnedDefenceItemsList == null)
        {
            _spawnedDefenceItemsList = new List<DefenceItem>();
        }

        DefenceItemData itemData = _itemsDatabase.ItemDataList.Find(x => x.Level == data.Level);

        DefenceItem item = _itemsPoolDictionary[itemData.Level].Get();

        item.Initialize(itemData);

        _spawnedDefenceItemsList.Add(item);

        StartCoroutine(FollowWhileInput(uiItem, item));
    }

    private IEnumerator FollowWhileInput(DefenceItemUIListElement uiItem, DefenceItem item)
    {
        Tile selectedTile = null;

        while (IsPressed())
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Vector3 screenPoint = new Vector3(mousePos.x, mousePos.y, Mathf.Abs(CameraController.Instance.MainCamera.transform.position.y - 1));
            Vector3 worldPos = CameraController.Instance.MainCamera.ScreenToWorldPoint(screenPoint);

            worldPos.y = 1;

            item.SetPosition(worldPos);

            Tile tile = SendRayForTileCheck(mousePos);

            if (tile != selectedTile)
            {
                selectedTile?.ShowDefault();
            }

            selectedTile = tile;

            yield return null;
        }

        selectedTile?.ShowDefault();

        if (selectedTile != null && selectedTile.CheckSnapAvailability())
        {
            GameManager.Instance.StartPlaying();

            item.AttachToTile(selectedTile);

            uiItem.OnPlaced();
        }
        else
        {
            _itemsPoolDictionary[item.DefenceItemData.Level].Return(item);

            uiItem.OnReturn();
        }
    }

    public void OnResetPool()
    {
        if (_spawnedDefenceItemsList != null)
        {
            foreach (DefenceItem item in _spawnedDefenceItemsList)
            {
                _itemsPoolDictionary[item.DefenceItemData.Level].Return(item);
            }

            _spawnedDefenceItemsList = new List<DefenceItem>();
        }
    }

    public void CreatePool()
    {
        _itemsPoolDictionary = new Dictionary<int, ObjectPool<DefenceItem>>();

        foreach (DefenceItemData data in _itemsDatabase.ItemDataList)
        {
            ObjectPool<DefenceItem> pool = new ObjectPool<DefenceItem>(data.Prefab, 3, _itemsContainer);

            if (_itemsPoolDictionary.ContainsKey(data.Level))
            {
                Debug.Log("There are more than one defence item data with same level!");

                continue;
            }

            _itemsPoolDictionary.Add(data.Level, pool);
        }
    }

    private bool IsPressed()
    {
        return (Mouse.current != null && Mouse.current.leftButton.isPressed)
            || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed);
    }

    private Tile SendRayForTileCheck(Vector2 mousePos)
    {
        Ray ray = CameraController.Instance.MainCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                if (tile.CheckSnapAvailability())
                {
                    tile.ShowAvailable();
                }
                else
                {
                    tile.ShowNotAvailable();
                }
            }

            return tile;
        }

        return null;
    }

    public void OnItemKilled(DefenceItem item)
    {
        _itemsPoolDictionary[item.DefenceItemData.Level].Return(item);
    }
}