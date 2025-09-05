using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

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
        }
    }

    [SerializeField] private float _bulletMoveTimeBetweenTiles = 0.2f;

    public void Spawn(DefenceItemData defenceItemData, Tile tile, Vector2Int direction)
    {
        _tilesMoved = 0;

        DefenceItemData = defenceItemData;

        AttachedTile = tile;

        transform.position = _attachedTile.transform.position + direction.y * 0.5f * Vector3.right + 0.5f * Vector3.up - direction.x * 0.5f * Vector3.forward;

        ToggleActive(true);

        StartCoroutine(MoveRoutine(direction));
        StartCoroutine(CheckHit(new Vector3(direction.y, 0, -direction.x)));
    }

    public IEnumerator SetPosition(Vector3 pos, float duration = 0)
    {
        if (duration <= 0)
        {
            transform.position = pos;
        }
        else
        {
            yield return transform.DOMove(pos, duration).WaitForCompletion();
        }
    }

    private IEnumerator MoveRoutine(Vector2Int direction)
    {
        while (Active && _tilesMoved < DefenceItemData.Range)
        {
            yield return StartCoroutine(MoveInDirection(direction));
        }

        BulletController.Instance.OnBulletDone(this);
    }

    private IEnumerator MoveInDirection(Vector2Int direction)
    {
        Tile tile = GridController.Instance.GetTileOnCoord(AttachedTile.Coord.x + direction.x, AttachedTile.Coord.y + direction.y);

        bool shouldMove = false;
        if (tile != null)
        {
            shouldMove = true;

            AttachedTile = tile;

            _tilesMoved++;

            yield return StartCoroutine(SetPosition(AttachedTile.transform.position + 0.5f * Vector3.up, 0.2f));
        }

        if (!shouldMove)
        {
            BulletController.Instance.OnBulletDone(this);
        }
    }

    public void ToggleActive(bool active)
    {
        Active = active;

        gameObject.SetActive(active);
    }

    private IEnumerator CheckHit(Vector3 direction)
    {
        while (Active)
        {
            RaycastHit hit;

            Debug.DrawRay(transform.position, direction);

            if (Physics.Raycast(transform.position, direction, out hit, 0.1f))
            {
                IDamagable damagable = hit.collider.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.TakeDamage(DefenceItemData.Damage);

                    BulletController.Instance.OnBulletDone(this);
                }
            }

            yield return null;
        }
    }
}