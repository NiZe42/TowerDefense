using System.Collections.Generic;
using UnityEngine;

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
    public int droppedMoney;
}

public struct OnPathHasChanged : IEvent
{
    public List<Vector3> newPath;
}

public struct OnEnemyReachedFinish : IEvent
{
    public int damage;
}

public struct OnWaveFinished : IEvent { }