using System;
using UnityEngine;

public abstract class MoveBehaviour : MonoBehaviour
{
    public abstract event Action OnReachedFinish;
    public abstract void Initialize(Enemy enemy);
    public abstract void Move(float speed);
}