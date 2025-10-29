using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    private static T instance = null;
    public static T Instance{ get=>instance; }
    
    public void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this as T;
    }
    
    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
