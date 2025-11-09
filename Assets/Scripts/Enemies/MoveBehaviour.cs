using System;
using UnityEngine;

/// <summary>
///     Abstract base class for enemy movement behaviours.
///     Defines the contract for initializing, moving, and signaling when the movement finishes.
/// </summary>
public abstract class MoveBehaviour : MonoBehaviour
{
    public abstract event Action OnReachedFinish;
    public abstract void Initialize(Enemy enemy);
    public abstract void Move(float speed);
}