using System.Collections;
using UnityEngine;

public class DefenceItem : PlacableItem, IKillable
{
    public DefenceItemData DefenceItemData { get; private set; }

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

            SetPosition(_attachedTile.transform.position + 0.5f * Vector3.up);
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

        IsAlive = true;

        StartCoroutine(ShootRoutine());
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private IEnumerator ShootRoutine()
    {
        while (IsAlive && GameManager.Instance.GameState == Enums.GameState.Playing)
        {
            yield return new WaitForSeconds(DefenceItemData.AttackInterval);

            switch (DefenceItemData.AttackDirection)
            {
                case Enums.DefenceItemAttackDir.All:

                    for (int i = 0; i < 4; i++)
                    {
                        TrySpawningOnCoord(AttachedTile.Coord.x + (int)Mathf.Cos(Mathf.Deg2Rad * (90f * i)), AttachedTile.Coord.y + (int)Mathf.Sin(Mathf.Deg2Rad * (90f * i)), new Vector2Int((int)Mathf.Cos(Mathf.Deg2Rad * (90f * i)), (int)Mathf.Sin(Mathf.Deg2Rad * (90f * i))));
                    }

                    break;
                case Enums.DefenceItemAttackDir.Forward:

                    TrySpawningOnCoord(AttachedTile.Coord.x - 1, AttachedTile.Coord.y, new Vector2Int(-1, 0));

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

    public void Kill()
    {
        IsAlive = false;

        DefenceItemPlaceController.Instance.OnItemKilled(this);
    }
}