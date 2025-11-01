using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IEvent {};

public struct FreeBlock2X2Selected : IEvent {
    public Vector2 blockCenter;
}

public struct TowerSelected : IEvent {
    public Vector2 towerCenter;
}

public struct EnemyDestroyed : IEvent {
    public int droppedMoney;
}

public struct PathHasChanged : IEvent {
    public List<Vector3> newPath;
}

public struct NoPossibleBoxFoundAfterClick : IEvent {}

