using System;

public class GameManager : Singleton<GameManager>
{
    private Enums.GameState _gameState;
    public Enums.GameState GameState
    {
        get
        {
            return _gameState;
        }
        private set
        {
            _gameState = value;

            OnGameStateChange();
        }
    }

    public Action OnGameStateChange;

    public void OnLevelLoaded()
    {
        GameState = Enums.GameState.Start;
    }

    public void StartPlaying()
    {
        if (GameState == Enums.GameState.Start)
        {
            GameState = Enums.GameState.Playing;
        }
    }

    public void OnGameEnd(bool success)
    {
        if (GameState != Enums.GameState.Playing)
        {
            return;
        }

        GameState = success ? Enums.GameState.Success : Enums.GameState.Fail;
    }
}