using UnityEngine;
using UnityEngine.UI;

public interface IEvent {};

public struct FreeBlock2X2Selected : IEvent {
    public Vector2 blockCenter;
}

public struct TowerSelected : IEvent {
    public Vector2 towerCenter;
}

public struct NoPossibleBoxFoundAfterClick : IEvent {}

