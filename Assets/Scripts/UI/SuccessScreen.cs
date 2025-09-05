using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : InGameScreen
{
    [SerializeField] private Button _nextLevelButton;

    private void Start()
    {
        _nextLevelButton.onClick.AddListener(NextLevel);
    }

    private void NextLevel()
    {

    }
}
