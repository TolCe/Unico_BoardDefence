using DG.Tweening;
using UnityEngine;

public class Bullet : PlacableItem
{
    public DefenceItemData DefenceItemData { get; private set; }

    private int _tilesMoved;

    private bool _isActive;

    private float _timer;

    private Tile _attachedTile;

    [SerializeField] private float _bulletMoveTimeBetweenTiles = 0.2f;

    private Vector2Int _direction;

    private Vector3 _lastPosition;

    private void Update()
    {
        if (_isActive)
        {
            if (_timer >= _bulletMoveTimeBetweenTiles)
            {
                MoveInDirection();

                _timer = 0f;
            }

            CheckHit();

            _timer += Time.deltaTime;
        }
    }

    public void Spawn(DefenceItemData defenceItemData, Tile tile, Vector2Int direction)
    {
        _tilesMoved = 0;
        _timer = 0f;

        DefenceItemData = defenceItemData;

        _direction = direction;

        _attachedTile = tile;

        SetPosition(_attachedTile.transform.position + 0.5f * Vector3.up);

        ToggleActive(true);
    }

    public void SetPosition(Vector3 pos, float duration = 0)
    {
        if (duration <= 0)
        {
            transform.position = pos;
        }
        else
        {
            transform.DOMove(pos, duration);
        }

        _lastPosition = transform.position;
    }

    private void MoveInDirection()
    {
        if (_tilesMoved >= DefenceItemData.Range)
        {
            OnBulletDone();

            return;
        }

        Tile tile = GridController.Instance.GetTileOnCoord(_attachedTile.Coord.x + _direction.x, _attachedTile.Coord.y + _direction.y);

        bool shouldMove = false;
        if (tile != null)
        {
            shouldMove = true;

            _attachedTile = tile;

            _tilesMoved++;

            SetPosition(_attachedTile.transform.position + 0.5f * Vector3.up, _bulletMoveTimeBetweenTiles);
        }

        if (!shouldMove)
        {
            OnBulletDone();
        }
    }

    public void ToggleActive(bool active)
    {
        _isActive = active;

        gameObject.SetActive(active);
    }

    private void CheckHit()
    {
        Vector3 direction = (transform.position - _lastPosition).normalized;
        float distance = Vector3.Distance(_lastPosition, transform.position);

        if (Physics.Raycast(_lastPosition, direction, out RaycastHit hit, distance))
        {
            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(DefenceItemData.Damage);
                OnBulletDone();
            }
        }

        _lastPosition = transform.position;
    }

    public void OnBulletDone()
    {
        _timer = 0f;
        _tilesMoved = 0;

        transform.DOKill();

        ToggleActive(false);

        _attachedTile = null;

        BulletController.Instance.OnBulletDone(this);
    }
}