using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefenceItem : PlacableItem, IDamagable
{
    public DefenceItemData DefenceItemData { get; private set; }

    [SerializeField] private Image _attackCountdownFillingImage;

    private bool _isAlive;

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

            SetPosition(_attachedTile.transform.position);
        }
    }

    public void Initialize(DefenceItemData data)
    {
        DefenceItemData = data;

        gameObject.SetActive(true);
    }

    public void AttachToTile(Tile tile)
    {
        AttachedTile = tile;

        _isAlive = true;

        StartCoroutine(ShootRoutine());
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private IEnumerator ShootRoutine()
    {
        float attackTimer = 0;

        while (_isAlive && GameManager.Instance.GameState == Enums.GameState.Playing)
        {
            attackTimer = 0;

            while (attackTimer < DefenceItemData.AttackInterval)
            {
                attackTimer += Time.deltaTime;

                _attackCountdownFillingImage.fillAmount = (DefenceItemData.AttackInterval - attackTimer) / DefenceItemData.AttackInterval;

                yield return new WaitForEndOfFrame();
            }

            switch (DefenceItemData.AttackDirection)
            {
                case Enums.DefenceItemAttackDir.All:

                    for (int i = 0; i < 4; i++)
                    {
                        TrySpawningOnCoord(AttachedTile.Coord.x, AttachedTile.Coord.y, new Vector2Int((int)Mathf.Cos(Mathf.Deg2Rad * (90f * i)), (int)Mathf.Sin(Mathf.Deg2Rad * (90f * i))));
                    }

                    break;
                case Enums.DefenceItemAttackDir.Forward:

                    TrySpawningOnCoord(AttachedTile.Coord.x, AttachedTile.Coord.y, new Vector2Int(-1, 0));

                    break;
                default:
                    break;
            }
        }
    }

    private void TrySpawningOnCoord(int row, int column, Vector2Int direction)
    {
        Tile tile = GridController.Instance.GetTileOnCoord(row, column);

        if (tile != null)
        {
            BulletController.Instance.SpawnBullet(DefenceItemData, tile, direction);
        }
    }

    public void TakeDamage(float damage)
    {
        Destroy();
    }

    public void Destroy()
    {
        _isAlive = false;

        AttachedTile?.RemoveItem();

        DefenceItemPlaceController.Instance.OnItemKilled(this);
    }
}