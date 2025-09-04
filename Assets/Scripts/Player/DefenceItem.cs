using UnityEngine;

public class DefenceItem : MonoBehaviour
{
    public DefenceItemData DefenceItemData { get; private set; }

    public void Initialize(DefenceItemData data)
    {
        DefenceItemData = data;

        gameObject.SetActive(true);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}