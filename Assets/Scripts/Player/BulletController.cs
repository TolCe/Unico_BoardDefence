using UnityEngine;

public class BulletController : SingletonMonoBehaviour<BulletController>
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletContainer;
    private ObjectPool<Bullet> _bulletPool;

    public void SpawnBullet(DefenceItemData defenceItemData, Tile tile, Vector2Int direction)
    {
        if (_bulletPool == null)
        {
            CreatePool();
        }

        Bullet bullet = _bulletPool.Get();

        bullet.Spawn(defenceItemData, tile, direction);
    }

    private void CreatePool()
    {
        _bulletPool = new ObjectPool<Bullet>(_bulletPrefab, 10, _bulletContainer);
    }

    public void OnBulletDone(Bullet bullet)
    {
        bullet.ToggleActive(false);

        _bulletPool.Return(bullet);
    }
}