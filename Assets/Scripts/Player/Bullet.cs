using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : PlacableItem
{
    public DefenceItemData DefenceItemData { get; private set; }

    private int _tilesMoved;

    public bool Active { get; private set; }

    private Tile _attachedTile;
    public Tile AttachedTile
    {
        get
        {
            return _attachedTile;
        }
        private set
        {
            _attachedTile = value;

            SetPosition(_attachedTile.transform.position + Vector3.up);
        }
    }

    public void Spawn(DefenceItemData defenceItemData, Tile tile, Vector2Int direction)
    {
        _tilesMoved = 0;

        DefenceItemData = defenceItemData;

        AttachedTile = tile;

        gameObject.SetActive(true);

        ToggleActive(true);

        StartCoroutine(MoveRoutine(direction));
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private IEnumerator MoveRoutine(Vector2Int direction)
    {
        while (Active && _tilesMoved < DefenceItemData.Range)
        {
            MoveInDirection(direction);

            yield return new WaitForSeconds(0.2f);
        }

        BulletController.Instance.OnBulletDone(this);
    }

    private void MoveInDirection(Vector2Int direction)
    {
        Tile tile = GridController.Instance.GetTileOnCoord(AttachedTile.Coord.x + direction.x, AttachedTile.Coord.y + direction.y);

        if (tile == null)
        {
            BulletController.Instance.OnBulletDone(this);
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
                    killable.TakeDamage(DefenceItemData.Damage);

                    shouldMove = true;
                }
                else
                {
                    BulletController.Instance.OnBulletDone(this);
                }
            }

            if (shouldMove)
            {
                AttachedTile = tile;

                _tilesMoved++;
            }
        }
    }

    public void ToggleActive(bool active)
    {
        Active = active;
    }
}