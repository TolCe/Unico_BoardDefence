using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2Int gridPosition; // Current row/col in the grid
    public float moveInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;
            MoveDown();
        }
    }

    void MoveDown()
    {
        gridPosition = new Vector2Int(gridPosition.x + 1, gridPosition.y);
        transform.position = GridToWorld(gridPosition);
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.y, -gridPos.x, 0f);
    }
}
