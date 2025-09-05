using DG.Tweening;
using System.Collections;
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
        }
    }

    public void SpawnEnemy(EnemyData data, Tile tile)
    {
        EnemyData = data;

        _timer = 0;

        IsAlive = true;

        CurrentHealth = EnemyData.Health;

        AttachedTile = tile;

        transform.position = AttachedTile.transform.position;

        gameObject.SetActive(true);

        ShowHealthbar();

        StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown()
    {
        while (IsAlive && GameManager.Instance.GameState == Enums.GameState.Playing)
        {
            Tile tile = GridController.Instance.GetTileOnCoord(AttachedTile.Coord.x + 1, AttachedTile.Coord.y);

            if (tile == null)
            {
                Debug.Log("Enemy target tile is null!");

                if (AttachedTile.Coord.x == GridController.Instance.Tiles.GetLength(0) - 1)
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

                    yield return StartCoroutine(SetPositionByTile(_attachedTile, 1f / EnemyData.Speed));
                }
            }
        }
    }

    private IEnumerator SetPositionByTile(Tile tile, float duration = 0)
    {
        if (duration <= 0)
        {
            transform.position = tile.transform.position;
        }
        else
        {
            yield return transform.DOMove(tile.transform.position, duration).WaitForCompletion();
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
        IsAlive = false;

        EnemySpawner.Instance.OnEnemyKilled(this);
    }
}
