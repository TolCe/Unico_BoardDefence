using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceItemUIListElement : MonoBehaviour
{
    public LevelDefenceItemData DefenceItemData { get; private set; }

    [SerializeField] private EventTrigger _eventTrigger;

    [SerializeField] private TMP_Text _levelText;

    public int CurrentCount { get; private set; }

    private void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDown(); });
        _eventTrigger.triggers.Add(entry);
    }

    private void OnPointerDown()
    {
        DefenceItemPlaceController.Instance.OnSelectedForPlacing(this, DefenceItemData);

        DecreaseCount();
    }

    public void Initialize(LevelDefenceItemData defenceItemData)
    {
        DefenceItemData = defenceItemData;

        CurrentCount = defenceItemData.Count;

        WriteAmount();

        transform.SetAsLastSibling();

        gameObject.SetActive(true);
    }

    private void WriteAmount()
    {
        _levelText.text = $"{CurrentCount}";
    }

    private void DecreaseCount()
    {
        CurrentCount--;

        WriteAmount();
    }

    public void OnPlaced()
    {
        if (CurrentCount <= 0)
        {
            DefenceItemsUIListing.Instance.ReturnToPool(this);
        }
    }

    public void OnReturn()
    {
        CurrentCount++;

        WriteAmount();
    }
}