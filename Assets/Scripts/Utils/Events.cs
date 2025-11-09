using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     Base class of all Events.
/// </summary>
public interface IEvent { }

public struct OnFreeBlock2X2Selected : IEvent
{
    public Vector3 blockCenter;
}

public struct OnTowerSelected : IEvent
{
    public int towerId;
}

public struct OnNothingSelected : IEvent { }

public struct OnEnemyDestroyed : IEvent
{
    public Vector3 deathPosition;
    public int droppedMoney;
}

public struct FinishedGeneratingQuads : IEvent { }

public struct OnPlayerMoneyChanged : IEvent
{
    public int newMoney;
}

public struct OnBuildStateStarted : IEvent { }

public struct OnPathHasChanged : IEvent
{
    public List<Vector3> newPath;
}

public struct OnWaveStarted : IEvent
{
    public int index;
}

public struct OnEnemyReachedFinish : IEvent
{
    public int damage;
}

public struct OnActiveEnemiesNumberChanged : IEvent
{
    public int newNumber;
}

public struct OnTowerBought : IEvent
{
    public int cost;
}

public struct OnTowerUpgraded : IEvent
{
    public int cost;
}

public struct OnTowerSold : IEvent
{
    public int moneyGained;
}

public struct OnPlayerHealthChanged : IEvent
{
    public int newPlayerHealth;
}

public struct OnAllWavesFinished : IEvent { }

public struct OnWaveFinished : IEvent { }