using System;

public class GameManager : Singleton<GameManager>
{
    public Enums.GameState GameState { get; private set; }

    public Action OnGameStateChange;

    public void StartPlaying()
    {
        if (GameState == Enums.GameState.Start)
        {
            GameState = Enums.GameState.Playing;

            OnGameStateChange?.Invoke();
        }
    }

    public void OnGameEnd(bool success)
    {
        if (GameState != Enums.GameState.Playing)
        {
            return;
        }

        GameState = success ? Enums.GameState.Success : Enums.GameState.Fail;

        OnGameStateChange?.Invoke();
    }
}