using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridPosition;

    private EnemyData _enemyData;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1f / _enemyData.Speed)
        {
            _timer = 0f;
            MoveDown();
        }
    }

    void MoveDown()
    {
        _gridPosition = new Vector2Int(_gridPosition.x + 1, _gridPosition.y);
        transform.position = GridToWorld(_gridPosition);
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.y, -gridPos.x, 0f);
    }
}
