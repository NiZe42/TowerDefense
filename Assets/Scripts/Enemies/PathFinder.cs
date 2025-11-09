using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathFinder
{
    private readonly TileGrid tileGrid;
    internal List<Tile> pathTiles;

    /// <summary>
    ///     Implements A* pathfinding on a <see cref="TileGrid" />.
    ///     Provides methods to calculate a path between two positions and check if it is still possible if some tiles are
    ///     blocked.
    /// </summary>
    internal PathFinder(TileGrid grid, Vector3 startPosition, Vector3 endPosition)
    {
        tileGrid = grid;

        if (!grid.TryGetTileIndexFromWorldPosition(
            startPosition,
            out Vector2Int startPositionIndex))
        {
            Debug.LogError("Could not find start position");
            return;
        }

        this.startPositionIndex = startPositionIndex;

        if (!grid.TryGetTileIndexFromWorldPosition(endPosition, out Vector2Int endPositionIndex))
        {
            Debug.LogError("Could not find end position");
            return;
        }

        this.endPositionIndex = endPositionIndex;
    }

    public Vector2Int startPositionIndex { get; internal set; }

    public Vector2Int endPositionIndex { get; internal set; }

    public bool IsPathStillValid(Vector3 blockedBLockCenterPosition)
    {
        if (!tileGrid.TryGetTileIndexesFromBlockCenterPosition(
            blockedBLockCenterPosition,
            out Vector2Int[] indexes))
        {
            Debug.Log("PathFinder: Could not find tile positions");
            return false;
        }

        int tilesWidth  = tileGrid.tiles.GetLength(0);
        int tilesHeight = tileGrid.tiles.GetLength(1);

        var temporaryBlockedMap = new bool[tilesWidth, tilesHeight];

        for (var x = 0; x < tilesWidth; x++)
        {
            for (var y = 0; y < tilesHeight; y++)
            {
                Tile tile = tileGrid.tiles[x, y];
                temporaryBlockedMap[x, y] = tile.isOccupied;
            }
        }

        foreach (Vector2Int tileIndex in indexes)
        {
            temporaryBlockedMap[tileIndex.x, tileIndex.y] = true;
        }

        return FindPath(temporaryBlockedMap) is not null;
    }

    // TODO make it so it recalculates more in favor of not changing path
    // If the path is the same distance, it should prefer one with most tiles that are in current path 
    internal List<Tile> FindPath(bool[,] blockedTiles = null)
    {
        Tile startTile = tileGrid.tiles[startPositionIndex.x, startPositionIndex.y];
        Tile endTile   = tileGrid.tiles[endPositionIndex.x, endPositionIndex.y];

        var tilesToExplore = new SimplePriorityQueue<Tile>();
        var cameFrom       = new Dictionary<Tile, Tile>();
        var gCost = new Dictionary<Tile, float>
        {
            [startTile] = 0
        };

        tilesToExplore.Enqueue(startTile, 0);

        while (tilesToExplore.Count > 0)
        {
            Tile currentTile = tilesToExplore.Dequeue();

            if (currentTile == endTile)
            {
                return Reconstruct(cameFrom, endTile);
            }

            if (!tileGrid.TryGetTileIndexFromTile(currentTile, out Vector2Int currentTileIndex))
            {
                Debug.LogError("Could not find tile index");
                return null;
            }

            // We check every neighbour, because we recalculate path only once in a while when map changes.
            // Because of that I choose possible safety above performance in this case.
            // If block tiles are provided, calculates path with them instead of real ones.
            foreach (Tile neighbour in tileGrid.GetNeighbours(currentTileIndex))
            {
                if (blockedTiles != null)
                {
                    if (!tileGrid.TryGetTileIndexFromTile(
                        neighbour,
                        out Vector2Int neighbourTileIndexCheck))
                    {
                        Debug.LogError("Could not find tile index for neighbour");
                        return null;
                    }

                    if (blockedTiles[neighbourTileIndexCheck.x, neighbourTileIndexCheck.y])
                    {
                        continue;
                    }
                }
                else
                {
                    if (neighbour.isOccupied)
                    {
                        continue;
                    }
                }

                if (!tileGrid.TryGetTileIndexFromTile(neighbour, out Vector2Int neighbourTileIndex))
                {
                    Debug.LogError("Could not find tile index for neighbour");
                    return null;
                }

                // path from a corner to corner should be diagonal rather than hugging the wall
                float currentToNeighbourCost = Vector2Int.Distance(
                    currentTileIndex,
                    neighbourTileIndex);

                float newNeighbourGCost = gCost[currentTile] + currentToNeighbourCost;

                if (!gCost.TryGetValue(neighbour, out float previousNeighbourGCost) ||
                    newNeighbourGCost < previousNeighbourGCost)
                {
                    gCost[neighbour]    = newNeighbourGCost;
                    cameFrom[neighbour] = currentTile;

                    float newNeighbourFCost = newNeighbourGCost +
                        Vector2Int.Distance(neighbourTileIndex, endPositionIndex);

                    tilesToExplore.Enqueue(neighbour, newNeighbourFCost);
                }
            }
        }

        return null;
    }

    private List<Tile> Reconstruct(Dictionary<Tile, Tile> cameFrom, Tile currentTile)
    {
        var result = new List<Tile>();

        while (cameFrom.ContainsKey(currentTile))
        {
            result.Add(currentTile);
            currentTile = cameFrom[currentTile];
        }

        result.Add(currentTile);
        result.Reverse();
        return result;
    }
}