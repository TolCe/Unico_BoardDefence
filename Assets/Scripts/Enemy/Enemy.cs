using UnityEngine;
using UnityEngine.UI;

public class Enemy : PlacableItem, IDamagable
{
    public EnemyData EnemyData { get; private set; }

    private float _timer;

    [SerializeField] private Image _healthFillingImage;

    public float CurrentHealth { get; private set; }

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

        CurrentHealth = EnemyData.Health;

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
                IDamagable killable = tile.AttachedItem.GetComponent<IDamagable>();
                if (killable != null)
                {
                    killable.TakeDamage(1);

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
        transform.position = tile.transform.position;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        _healthFillingImage.fillAmount = CurrentHealth / EnemyData.Health;

        if (CurrentHealth <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        IsAlive = false;

        EnemySpawner.Instance.OnEnemyKilled(this);
    }
}
