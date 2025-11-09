using System;
using UnityEngine;

/// <summary>
///     An object that can take damage in the game.
///     Unity cannot serialize interfaces in the inspector, so an abstract class
///     is used for editor interaction.
/// </summary>
public abstract class Damageable : MonoBehaviour
{
    public abstract int Health { get; protected set; }
    public abstract int MaxHealth { get; protected set; }
    public abstract event Action<int, int> OnDamageTaken;
    public abstract void ReceiveDamage(int damage);
    public abstract void Died();
}