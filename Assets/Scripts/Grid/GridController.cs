using System;
using System.Collections.Generic;
using UnityEngine;

public class GridController : SingletonMonoBehaviour<GridController>, IPooler
{
    public Tile[,] Tiles { get; private set; }

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _tileContainer;

    private ObjectPool<Tile> _tilesPool;

    protected override void Awake()
    {
        base.Awake();

        LevelLoader.Instance.OnReset += OnResetPool;
    }

    public void SetTileArraySize(int row, int column)
    {
        Tiles = new Tile[row, column];
    }

    public void CreateGrid(int row, int column)
    {
        _tilesPool?.ReturnAll();

        SetTileArraySize(row, column);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector3 pos = new Vector3(j, 0, -i);

                Tile tile = CreateTile();

                Tiles[i, j] = tile;

                PlaceTile(tile, new Vector2Int(i, j), pos);
            }
        }

        float tilesMidXPoint = (Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 1].transform.position.x + Tiles[0, 0].transform.position.x) * 0.5f;
        float tilesMidZPoint = (Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 1].transform.position.z + Tiles[0, 0].transform.position.z) * 0.5f;

        CameraController.Instance.SetCameraPosition(new Vector3(tilesMidXPoint, 0, tilesMidZPoint - 4f));
    }

    public Tile CreateTile()
    {
        if (_tilesPool == null)
        {
            CreatePool();
        }

        return _tilesPool.Get();
    }

    public void OnResetPool()
    {
        if (_tilesPool != null)
        {
            _tilesPool.ReturnAll();
        }
    }

    public void CreatePool()
    {
        _tilesPool = new ObjectPool<Tile>(_tilePrefab, 20, _tileContainer);
    }

    public void PlaceTile(Tile tile, Vector2Int coord, Vector3 pos)
    {
        tile.Initialize(coord);

        tile.transform.position = pos;

        tile.gameObject.SetActive(true);
    }

    public Tile GetTileOnCoord(int row, int column)
    {
        if (row < 0 || column < 0)
        {
            return null;
        }

        if (row >= Tiles.GetLength(0) || column >= Tiles.GetLength(1))
        {
            return null;
        }

        return Tiles[row, column];
    }

    public Tile GetRandomTileFromFirstRow()
    {
        List<Tile> emptyTilesList = new List<Tile>();
        for (int i = 0; i < Tiles.GetLength(1); i++)
        {
            if (Tiles[0, i].AttachedItem == null)
            {
                emptyTilesList.Add(Tiles[0, i]);
            }
        }

        if (emptyTilesList.Count <= 0)
        {
            Debug.LogWarning("There is no available tile to spawn enemies!");

            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyTilesList.Count);

        return emptyTilesList[randomIndex];
    }
}