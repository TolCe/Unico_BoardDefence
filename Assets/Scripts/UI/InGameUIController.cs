using System.Collections.Generic;
using System;
using UnityEngine;

public class InGameUIController : SingletonMonoBehaviour<InGameUIController>
{
    [SerializeField] private List<InGameScreen> _screenList;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void OnGameStateChange()
    {
        switch (GameManager.Instance.GameState)
        {
            case Enums.GameState.Start:

                _screenList.Find(x => x.ScreenType == Enums.InGameScreenTypes.Start).ToggleScreen(true);
                _screenList.Find(x => x.ScreenType == Enums.InGameScreenTypes.Success).ToggleScreen(false);
                _screenList.Find(x => x.ScreenType == Enums.InGameScreenTypes.Fail).ToggleScreen(false);

                break;
            case Enums.GameState.Playing:

                _screenList.Find(x => x.ScreenType == Enums.InGameScreenTypes.Start).ToggleScreen(false);

                break;
            case Enums.GameState.Success:

                _screenList.Find(x => x.ScreenType == Enums.InGameScreenTypes.Success).ToggleScreen(true);

                break;
            case Enums.GameState.Fail:

                _screenList.Find(x => x.ScreenType == Enums.InGameScreenTypes.Fail).ToggleScreen(true);

                break;
            default:
                break;
        }
    }
}
