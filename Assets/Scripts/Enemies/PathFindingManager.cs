using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Manages pathfinding logic for enemies on a <see cref="TileGrid" />.
///     Provides access to the current path and allows recalculation when the grid changes.
/// </summary>
[RequireComponent(typeof(PathVisualizer))]
public class PathFindingManager : MonoBehaviourSingleton<PathFindingManager>, IPathValidator
{
    internal static Action<List<Tile>> OnPathUpdated;

    [SerializeField]
    private PathVisualizer pathVisualizer;

    internal List<Tile> currentPath = new List<Tile>();
    private Transform finishPosition;

    private PathFinder pathFinder;

    private Transform startTransform;

    internal TileGrid tileGrid;

    public void Start()
    {
        tileGrid       = TileGrid.Instance;
        startTransform = GameObject.FindGameObjectWithTag("StartPosition").transform;
        finishPosition = GameObject.FindGameObjectWithTag("FinishPosition").transform;
        pathFinder     = new PathFinder(tileGrid, startTransform.position, finishPosition.position);
        currentPath    = pathFinder.FindPath();

        Debug.Log("Intialized path finding");
        pathVisualizer.Initialize(tileGrid, currentPath);

        // OnPathUpdated?.Invoke(currentPath);

        EventBus.Instance.Subscribe<OnTowerBought>(RecalculatePath);
        EventBus.Instance.Subscribe<OnTowerSold>(RecalculatePath);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventBus.Instance.Unsubscribe<OnTowerBought>(RecalculatePath);
        EventBus.Instance.Unsubscribe<OnTowerSold>(RecalculatePath);
    }

    public bool IsPathStillValid(Vector3 blockedBlockCenterPosition)
    {
        return pathFinder.IsPathStillValid(blockedBlockCenterPosition);
    }

    public List<Vector3> GetCurrentPath()
    {
        return currentPath.Select(tile => tile.worldPosition).ToList();
    }

    public void RecalculatePath()
    {
        currentPath = pathFinder.FindPath();

        EventBus.Instance.InvokeEvent(
            new OnPathHasChanged
                { newPath = currentPath.Select(tile => tile.worldPosition).ToList() });

        // We need to pass List<Tiles> into path visualizer.
        // I think Tile is strictly a grid property, so I created "local" event.
        OnPathUpdated?.Invoke(currentPath);
    }
}