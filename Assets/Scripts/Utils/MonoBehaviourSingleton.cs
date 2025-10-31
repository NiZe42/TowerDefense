using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    private static T instance;
    public static T Instance{ get=>instance; }
    
    public virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this as T;
    }
    
    public virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
