using UnityEngine;

/// <summary>
/// A generic Singleton class for creating single instances of a MonoBehaviour.
/// </summary>
/// <typeparam name="T">Type of the Singleton class.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<T>();
            if (_instance != null) return _instance;

            var singletonObject = new GameObject();
            _instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T) + " (Singleton)";
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
