using UnityEngine;

public class Tile : MonoBehaviour
{
    public PlacableItem AttachedItem { get; private set; }

    public Vector2Int Coord { get; private set; }

    [SerializeField] private Renderer _rend;

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _availableMaterial;
    [SerializeField] private Material _notAvailableMaterial;

    public void Initialize(Vector2Int coord)
    {
        Coord = coord;

        RemoveItem();
    }

    public void AttachItem(PlacableItem placable)
    {
        AttachedItem = placable;
    }

    public void RemoveItem()
    {
        AttachedItem = null;
    }

    public void ShowDefault()
    {
        _rend.material = _defaultMaterial;
    }

    public void ShowAvailable()
    {
        _rend.material = _availableMaterial;
    }

    public void ShowNotAvailable()
    {
        _rend.material = _notAvailableMaterial;
    }

    public bool CheckSnapAvailability()
    {
        return Coord.x >= GridController.Instance.Tiles.GetLength(0) * 0.5f && AttachedItem == null;
    }
}