using UnityEngine;

public class Tile : MonoBehaviour
{
    public Enemy AttachedEnemy { get; private set; }

    public Vector2Int Coord { get; private set; }

    public void Initialize(Vector2Int coord)
    {
        Coord = coord;
    }

    public void AttachEnemy(Enemy enemy)
    {
        AttachedEnemy = enemy;
    }

    public void RemoveEnemy()
    {
        AttachedEnemy = null;
    }
}