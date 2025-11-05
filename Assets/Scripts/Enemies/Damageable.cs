using System;
using UnityEngine;

// I would like this to be interface, but Unity does not allow for serialization of objects as interfaces.
// So for editor interaction it is abstract class.
public abstract class Damageable : MonoBehaviour
{
    public abstract int Health { get; protected set; }
    public abstract int MaxHealth { get; protected set; }
    public abstract event Action<int, int> OnDamageTaken;
    public abstract void ReceiveDamage(int damage);
    public abstract void Died();
}