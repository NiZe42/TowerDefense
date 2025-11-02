using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Designed to have only 1 grid in the world.
public class PathFindingManager : MonoBehaviourSingleton<PathFindingManager>, IPathValidator
{
    internal static Action<List<Tile>> OnPathUpdated;
    internal List<Tile> currentPath = new List<Tile>();
    private Transform finishPosition;

    private PathFinder pathFinder;

    private Transform startTransform;

    internal TileGrid tileGrid;

    public void Start()
    {
        tileGrid       = FindAnyObjectByType<TileGrid>();
        startTransform = GameObject.FindGameObjectWithTag("StartPosition").transform;
        finishPosition = GameObject.FindGameObjectWithTag("FinishPosition").transform;
        pathFinder     = new PathFinder(tileGrid, startTransform.position, finishPosition.position);
        currentPath    = pathFinder.FindPath();
        OnPathUpdated?.Invoke(currentPath);
        EventBus.Instance.InvokeEvent(
            new OnPathHasChanged
                { newPath = currentPath.Select(tile => tile.worldPosition).ToList() });
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("pressed");
            currentPath = pathFinder.FindPath();

            EventBus.Instance.InvokeEvent(
                new OnPathHasChanged
                    { newPath = currentPath.Select(tile => tile.worldPosition).ToList() });

            // We need to pass List<Tiles> into path visualizer.
            // I think Tile is strictly a grid property, so I created "local" event.
            OnPathUpdated?.Invoke(currentPath);
        }
    }

    public bool IsPathStillValid(Vector3 blockedBlockCenterPosition)
    {
        return pathFinder.IsPathStillValid(blockedBlockCenterPosition);
    }

    public List<Vector3> GetCurrentPath()
    {
        return currentPath.Select(tile => tile.worldPosition).ToList();
    }
}