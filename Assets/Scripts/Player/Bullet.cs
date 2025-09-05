using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : PlacableItem
{
    public DefenceItemData DefenceItemData { get; private set; }

    private int _tilesMoved;

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

            SetPosition(_attachedTile.transform.position + Vector3.up);
        }
    }

    public void Spawn(DefenceItemData defenceItemData, Tile tile, Vector2Int direction)
    {
        _tilesMoved = 0;

        DefenceItemData = defenceItemData;

        AttachedTile = tile;

        gameObject.SetActive(true);

        StartCoroutine(MoveRoutine(direction));
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private IEnumerator MoveRoutine(Vector2Int direction)
    {
        while (gameObject.activeSelf && _tilesMoved < DefenceItemData.Range)
        {
            yield return new WaitForSeconds(0.5f);

            MoveInDirection(direction);
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
                IKillable killable = tile.AttachedItem.GetComponent<IKillable>();
                if (killable != null)
                {
                    killable.Kill();

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
}