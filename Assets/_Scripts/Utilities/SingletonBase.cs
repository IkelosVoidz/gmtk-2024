using UnityEngine;

//Generic implementation (so that it can work on any monobehaviour) of a base singleton abstract class ,
//based on Tarodev's youtube series on how to use singletons for a better game architechture
//Modified slightly to fit our project  


/// <summary>
/// A static instance is similar to a singleton , but instead of destroying any new
/// instances , it overrides the current instance. This is handy for resetting the state 
/// and saves you doing it manually
/// </summary>
public abstract class StaticSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;
    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Finally we have a persistent version of the singleton. This will survive through scene loads.
/// Perfect for system classes which require stateful, persistent data.d
/// Or audio sources where music plays through loading scenes, etc
/// </summary>
public abstract class PersistentSingleton<T> : StaticSingleton<T> where T : MonoBehaviour
{

    protected override void Awake()
    {
        if (Instance == null)
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}