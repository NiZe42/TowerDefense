using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable {
    public override event Action<int, int> OnDamageTaken;

    [SerializeField]
    protected int maxHealth;

    [SerializeField]
    protected int baseMoveSpeed;

    [SerializeField]
    protected int carriedMoney;

    [SerializeField]
    protected MoveBehaviour moveBehaviour;
    
    [SerializeField]
    protected Animator animator;
    
    protected int moveSpeedMultiplier;

    public override int MaxHealth { get => maxHealth; protected set => maxHealth = value; }

    private int health;
    public override int Health { get => health; protected set => health = value; }

    public void OnEnable() {
        health = 5;
    }

    public void Start() {
        moveBehaviour.Initialize(this);
    }

    private void FixedUpdate() {
        moveBehaviour.Move(baseMoveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime);
    }

    public override void ReceiveDamage(int damage) {
        health -= damage;
        DamageTaken();
        if (health > 0) return;
        
        health = 0;
        Died();
    }

    private void DamageTaken() {
        OnDamageTaken?.Invoke(health, maxHealth);
    }    
    public override void Died() {
        EventBus.Instance.InvokeEvent(new EnemyDestroyed(){droppedMoney = this.carriedMoney});
        Destroy(gameObject);
    }
}
