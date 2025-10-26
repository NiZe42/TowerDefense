using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    private static T instance = new T();
    public static T Instance => instance;
}
