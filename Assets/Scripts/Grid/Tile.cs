using UnityEngine;

public class Tile : MonoBehaviour
{
    public Enemy AttachedEnemy { get; private set; }

    public void AttachEnemy(Enemy enemy)
    {
        AttachedEnemy = enemy;
    }

    public void RemoveEnemy()
    {
        AttachedEnemy = null;
    }
}