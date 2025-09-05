using UnityEngine;

public class InGameScreen : MonoBehaviour
{
    [SerializeField] private Enums.InGameScreenTypes _screenType;
    public Enums.InGameScreenTypes ScreenType { get { return _screenType; } }

    public void ToggleScreen(bool state)
    {
        gameObject.SetActive(state);
    }
}
