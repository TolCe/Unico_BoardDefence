using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : PlacableItem, IDamagable
{
    public EnemyData EnemyData { get; private set; }

    [SerializeField] private Image _healthFillingImage;

    private float _timer;

    public float CurrentHealth { get; private set; }

    private Tile _attachedTile;

    private void Update()
    {
        if (_timer >= 1f / EnemyData.Speed)
        {
            MoveDown();

            _timer = 0f;
        }

        _timer += Time.deltaTime;
    }

    public void SpawnEnemy(EnemyData data, Tile tile)
    {
        _timer = 0;

        EnemyData = data;

        CurrentHealth = EnemyData.Health;

        _attachedTile = tile;
        _attachedTile.AttachItem(this);

        SetPosition(_attachedTile);

        gameObject.SetActive(true);

        ShowHealthbar();
    }

    private void MoveDown()
    {
        Tile tile = GridController.Instance.GetTileOnCoord(_attachedTile.Coord.x + 1, _attachedTile.Coord.y);

        if (tile == null)
        {
            Debug.Log("Enemy target tile is null!");

            if (_attachedTile.Coord.x == GridController.Instance.Tiles.GetLength(0) - 1)
            {
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
                if (killable != null && killable is not Enemy)
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
                _attachedTile?.RemoveItem();
                _attachedTile = tile;
                _attachedTile.AttachItem(this);

                SetPosition(_attachedTile, 1f / EnemyData.Speed);
            }
        }
    }

    private void SetPosition(Tile tile, float duration = 0f)
    {
        if (duration <= 0f)
        {
            transform.position = tile.transform.position;
        }
        else
        {
            transform.DOMove(tile.transform.position, duration);
        }
    }

    private void ShowHealthbar()
    {
        _healthFillingImage.fillAmount = CurrentHealth / EnemyData.Health;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        ShowHealthbar();

        if (CurrentHealth <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        _attachedTile?.RemoveItem();
        _attachedTile = null;

        transform.DOKill();

        EnemySpawner.Instance.OnEnemyKilled(this);
    }
}
