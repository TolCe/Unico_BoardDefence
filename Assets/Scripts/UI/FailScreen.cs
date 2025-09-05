using System;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : InGameScreen
{
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(RestartLevel);
    }

    private void RestartLevel()
    {
        LevelLoader.Instance.OnLevelRestart();
    }
}
