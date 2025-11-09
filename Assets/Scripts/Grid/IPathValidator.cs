using UnityEngine;

/// <summary>
///     Dependency Injection that provides pathfinding services.
/// </summary>
public interface IPathValidator
{
    public bool IsPathStillValid(Vector3 blockedTileWorldPosition);
}