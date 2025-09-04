using System;
using System.Collections.Generic;
using UnityEngine;

public class GridController : SingletonMonoBehaviour<GridController>
{
    public Tile[,] _tiles { get; private set; }

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _tileContainer;

    private ObjectPool<Tile> _tilesPool;

    public void SetTileArraySize(int row, int column)
    {
        _tiles = new Tile[row, column];
    }

    public void CreateGrid(int row, int column)
    {
        SetTileArraySize(row, column);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector3 pos = new Vector3(j, 0, -i);

                Tile tile = CreateTile();

                _tiles[i, j] = tile;

                PlaceTile(tile, new Vector2Int(i, j), pos);
            }
        }

        float tilesMidXPoint = (_tiles[_tiles.GetLength(0) - 1, _tiles.GetLength(1) - 1].transform.position.x + _tiles[0, 0].transform.position.x) * 0.5f;
        float tilesMidZPoint = (_tiles[_tiles.GetLength(0) - 1, _tiles.GetLength(1) - 1].transform.position.z + _tiles[0, 0].transform.position.z) * 0.5f;

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

    private void CreatePool()
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
        if (row >= _tiles.GetLength(0) || column >= _tiles.GetLength(1))
        {
            return null;
        }

        return _tiles[row, column];
    }

    public Tile GetRandomTileFromFirstRow()
    {
        List<Tile> emptyTilesList = new List<Tile>();
        for (int i = 0; i < _tiles.GetLength(1); i++)
        {
            if (_tiles[0, i].AttachedEnemy == null)
            {
                emptyTilesList.Add(_tiles[0, i]);
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyTilesList.Count);

        return emptyTilesList[randomIndex];
    }
}