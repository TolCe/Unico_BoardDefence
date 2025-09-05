using UnityEngine;

public class Enemy : PlacableItem, IKillable
{
    [SerializeField] private Vector2Int _gridPosition;

    public EnemyData EnemyData { get; private set; }

    private float _timer;

    private bool _isAlive;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }

        private set
        {
            _isAlive = value;

            AttachedTile?.RemoveItem();
        }
    }

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

            _attachedTile.AttachItem(this);

            SetPositionByTile(_attachedTile);
        }
    }

    private void Update()
    {
        if (IsAlive && GameManager.Instance.GameState == Enums.GameState.Playing)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1f / EnemyData.Speed)
            {
                _timer = 0f;
                MoveDown();
            }
        }
    }

    public void SpawnEnemy(EnemyData data, Tile tile)
    {
        EnemyData = data;

        _timer = 0;

        IsAlive = true;

        gameObject.SetActive(true);

        AttachedTile = tile;
    }

    private void MoveDown()
    {
        Tile tile = GridController.Instance.GetTileOnCoord(AttachedTile.Coord.x + 1, AttachedTile.Coord.y);

        if (tile == null)
        {
            Debug.Log("Enemy target tile is null!");

            if (AttachedTile.Coord.x == GridController.Instance.Tiles.GetLength(0) - 1)
            {
                Debug.Log("GAME OVER!");

                GameManager.Instance.OnGameEnd(false);
            }
        }
        else
        {
            bool shouldMove = false;
            if (tile.AttachedItem == null)
            {
                shouldMove = true;
            }
            else
            {
                IKillable killable = tile.AttachedItem.GetComponent<IKillable>();
                if (killable != null)
                {
                    killable.Kill();

                    shouldMove = true;
                }
                else
                {
                    Debug.Log("Enemy target tile is not empty!");
                }
            }

            if (shouldMove)
            {
                AttachedTile = tile;
            }
        }
    }

    private void SetPositionByTile(Tile tile)
    {
        transform.position = tile.transform.position + Vector3.up;
    }

    public void Kill()
    {
        IsAlive = false;

        EnemySpawner.Instance.OnEnemyKilled(this);
    }
}
