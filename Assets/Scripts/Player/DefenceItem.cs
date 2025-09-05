using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefenceItem : PlacableItem, IDamagable
{
    public DefenceItemData DefenceItemData { get; private set; }

    [SerializeField] private Image _attackCountdownFillingImage;

    private float _timer;

    private Tile _attachedTile;

    private void Update()
    {
        if (_timer >= DefenceItemData.AttackInterval)
        {
            Shoot();

            _timer = 0;
        }

        if (_attachedTile != null)
        {
            _timer += Time.deltaTime;
        }

        _attackCountdownFillingImage.fillAmount = (DefenceItemData.AttackInterval - _timer) / DefenceItemData.AttackInterval;
    }

    public void Initialize(DefenceItemData data)
    {
        DefenceItemData = data;

        gameObject.SetActive(true);
    }

    public void AttachToTile(Tile tile)
    {
        _attachedTile = tile;
        _attachedTile.AttachItem(this);

        SetPosition(tile.transform.position);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Shoot()
    {
        switch (DefenceItemData.AttackDirection)
        {
            case Enums.DefenceItemAttackDir.All:

                for (int i = 0; i < 4; i++)
                {
                    TrySpawningOnCoord(_attachedTile.Coord.x, _attachedTile.Coord.y, new Vector2Int((int)Mathf.Cos(Mathf.Deg2Rad * (90f * i)), (int)Mathf.Sin(Mathf.Deg2Rad * (90f * i))));
                }

                break;
            case Enums.DefenceItemAttackDir.Forward:

                TrySpawningOnCoord(_attachedTile.Coord.x, _attachedTile.Coord.y, new Vector2Int(-1, 0));

                break;
            default:
                break;
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
        _timer = 0f;

        _attachedTile?.RemoveItem();
        _attachedTile = null;

        _attackCountdownFillingImage.fillAmount = 1f;

        DefenceItemPlaceController.Instance.OnItemKilled(this);
    }
}