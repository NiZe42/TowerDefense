using UnityEngine;

// Dislike it very much, but i do not know hot to get Grid to know about pathfinding in other ways.
public interface IPathValidator
{
    public bool IsPathStillValid(Vector3 blockedTileWorldPosition);
}