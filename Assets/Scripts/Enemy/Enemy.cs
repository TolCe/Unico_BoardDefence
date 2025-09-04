using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridPosition;

    private EnemyData _enemyData;

    private float _timer;

    private Tile _attachedTile;
    public Tile AttachedTile
    {
        get
        {
            return _attachedTile;
        }
        private set
        {
            AttachedTile?.RemoveItem();

            _attachedTile = value;

            _attachedTile.AttachItem();

            SetPositionByTile(_attachedTile);
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1f / _enemyData.Speed)
            {
                _timer = 0f;
                MoveDown();
            }
        }
    }

    public void SpawnEnemy(EnemyData data, Tile tile)
    {
        _enemyData = data;

        _timer = 0;

        gameObject.SetActive(true);

        AttachedTile = tile;
    }

    private void MoveDown()
    {
        Tile tile = GridController.Instance.GetTileOnCoord(AttachedTile.Coord.x + 1, AttachedTile.Coord.y);

        if (tile == null)
        {
            Debug.Log("Enemy target tile is null!");
        }
        else
        {
            if (tile.IsEmpty)
            {
                AttachedTile = tile;
            }
            else
            {
                Debug.Log("Enemy target tile is not empty!");
            }
        }
    }

    private void SetPositionByTile(Tile tile)
    {
        transform.position = tile.transform.position + Vector3.up;
    }
}
