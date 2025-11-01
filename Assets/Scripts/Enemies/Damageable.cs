using System;
using UnityEngine;

public abstract class Damageable : MonoBehaviour {
    public abstract event Action<int, int> OnDamageTaken;
    public abstract int Health {get; protected set; }
    public abstract int MaxHealth {get; protected set; }
    public abstract void ReceiveDamage(int damage);
    public abstract void Died();
}
