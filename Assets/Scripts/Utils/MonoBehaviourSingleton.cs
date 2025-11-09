using UnityEngine;

/// <summary>
///     Singleton, that ensures that it is the only one object in loaded scenes.
/// </summary>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this as T;
    }

    public virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}