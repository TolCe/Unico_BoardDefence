using System.Collections.Generic;
using UnityEngine;

public class BulletController : SingletonMonoBehaviour<BulletController>, IPooler
{
    [SerializeField] private Bullet _bulletPrefab;

    [SerializeField] private Transform _bulletContainer;

    private ObjectPool<Bullet> _bulletPool;

    private List<Bullet> _spawnedBulletList = new List<Bullet>();

    protected override void Awake()
    {
        base.Awake();

        LevelLoader.Instance.OnReset += OnResetPool;
    }

    public void SpawnBullet(DefenceItemData defenceItemData, Tile tile, Vector2Int direction)
    {
        if (_bulletPool == null)
        {
            CreatePool();
        }

        if (_spawnedBulletList == null)
        {
            _spawnedBulletList = new List<Bullet>();
        }

        Bullet bullet = _bulletPool.Get();

        bullet.Spawn(defenceItemData, tile, direction);

        _spawnedBulletList.Add(bullet);
    }

    public void OnResetPool()
    {
        foreach (Bullet bullet in _spawnedBulletList)
        {
            bullet.OnBulletDone();
        }
    }

    public void CreatePool()
    {
        _bulletPool = new ObjectPool<Bullet>(_bulletPrefab, 10, _bulletContainer);
    }

    public void OnBulletDone(Bullet bullet)
    {
        _bulletPool.Return(bullet);
    }
}