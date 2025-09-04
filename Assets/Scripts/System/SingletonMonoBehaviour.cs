using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)Object.FindFirstObjectByType(typeof(T));
            }
            if (_instance == null)
            {
                Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
}
