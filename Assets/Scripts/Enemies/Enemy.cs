using System;
using UnityEngine;

/// <summary>
///     An enemy in the game that can move, take damage, and interact with the game world.
///     Inherits from <see cref="Damageable" /> to have health.
/// </summary>
public class Enemy : Damageable
{
    [SerializeField]
    protected int maxHealth;

    [SerializeField]
    protected float baseMoveSpeed;

    [SerializeField]
    protected int carriedMoney;

    [SerializeField]
    protected int finishDamage;

    [SerializeField]
    protected MoveBehaviour moveBehaviour;

    [SerializeField]
    private GameObject visualPrefab;

    public float moveSpeedMultiplier = 1;

    private int health;

    private EnemyVisual visualInstance;

    public override int MaxHealth { get => maxHealth; protected set => maxHealth = value; }
    public override int Health { get => health; protected set => health = value; }

    private void Awake()
    {
        visualInstance = Instantiate(visualPrefab, transform).GetComponent<EnemyVisual>();
    }

    private void Start()
    {
        moveBehaviour.Initialize(this);
        moveBehaviour.OnReachedFinish += ReachedFinish;
    }

    private void FixedUpdate()
    {
        moveBehaviour.Move(baseMoveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime);
        visualInstance.speedMultiplier = moveSpeedMultiplier;
    }

    public void OnEnable()
    {
        health = maxHealth;
    }

    public override event Action<int, int> OnDamageTaken;

    public override void ReceiveDamage(int damage)
    {
        health -= damage;
        DamageTaken();
        if (health > 0)
        {
            return;
        }

        health = 0;
        Died();
    }

    private void DamageTaken()
    {
        OnDamageTaken?.Invoke(health, maxHealth);
    }

    private void ReachedFinish()
    {
        EventBus.Instance.InvokeEvent(new OnEnemyReachedFinish { damage = finishDamage });
        Destroy(gameObject);
    }

    public override void Died()
    {
        EventBus.Instance.InvokeEvent(
            new OnEnemyDestroyed
                { droppedMoney = carriedMoney, deathPosition = transform.position });

        Destroy(gameObject);
    }
}