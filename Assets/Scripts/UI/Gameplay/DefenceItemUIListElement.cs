using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceItemUIListElement : MonoBehaviour
{
    public DefenceItemData DefenceItemData { get; private set; }

    [SerializeField] private EventTrigger _eventTrigger;

    [SerializeField] private TMP_Text _levelText;

    private void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDown(); });
        _eventTrigger.triggers.Add(entry);
    }

    private void OnPointerDown()
    {
        DefenceItemPlaceController.Instance.OnSelectedForPlacing(DefenceItemData);
    }

    public void Initialize(DefenceItemData defenceItemData)
    {
        DefenceItemData = defenceItemData;

        WriteLevel();

        gameObject.SetActive(true);
    }

    private void WriteLevel()
    {
        _levelText.text = $"{DefenceItemData.Level}";
    }
}