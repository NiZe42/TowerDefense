using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Priority_Queue;
using Vector3 = UnityEngine.Vector3;

public class PathFinder
{
    TileGrid tileGrid;
    
    internal List<Tile> pathTiles;

    public Vector2Int startPositionIndex { get; internal set; }
    public Vector2Int endPositionIndex { get; internal set; }

    internal PathFinder(TileGrid grid, Vector3 startPosition, Vector3 endPosition) {
        tileGrid = grid;

        if (!grid.TryGetTileIndexFromWorldPosition(startPosition, out Vector2Int startPositionIndex)) {
            Debug.LogError("Could not find start position");
            return;
        }
        this.startPositionIndex = startPositionIndex;

        if (!grid.TryGetTileIndexFromWorldPosition(endPosition, out Vector2Int endPositionIndex)) {
            Debug.LogError("Could not find end position");
            return;
        }
        this.endPositionIndex = endPositionIndex;
    }
    
    internal List<Tile> FindPath()
    {
        Tile startTile = tileGrid.tiles[startPositionIndex.x, startPositionIndex.y];
        Tile endTile   = tileGrid.tiles[endPositionIndex.x, endPositionIndex.y];
        
        SimplePriorityQueue<Tile> tilesToExplore = new  SimplePriorityQueue<Tile>();
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> gCost = new Dictionary<Tile, float> {
            [startTile] = 0
        };

        tilesToExplore.Enqueue(startTile, 0);

        while (tilesToExplore.Count > 0)
        {
            Tile currentTile = tilesToExplore.Dequeue();

            if (currentTile == endTile)
                return Reconstruct(cameFrom, endTile);
            

            if (!tileGrid.TryGetTileIndexFromTile(currentTile, out Vector2Int currentTileIndex)) {
                Debug.LogError("Could not find tile index");
                return null;
            }

            // We check every neighbour, because we recalculate path only once in a while when map changes.
            // Because of that I choose possible safety above performance in this case.
            foreach (Tile neighbour in tileGrid.GetNeighbours(currentTileIndex))
            {
                if (neighbour.IsOccupied())
                    continue;

                if (!tileGrid.TryGetTileIndexFromTile(neighbour, out Vector2Int neighbourTileIndex)) {
                    Debug.LogError("Could not find tile index for neighbour");
                    return null;
                }
                
                // path from a corner to corner should be diagonal rather than hugging the wall
                float currentToNeighbourCost = Vector2Int.Distance(currentTileIndex, neighbourTileIndex);
                float newNeighbourGCost = gCost[currentTile] + currentToNeighbourCost;

                if (!gCost.TryGetValue(neighbour, out float previousNeighbourGCost) 
                    || newNeighbourGCost < previousNeighbourGCost)
                {
                    gCost[neighbour] = newNeighbourGCost;
                    cameFrom[neighbour] = currentTile;

                    float newNeighbourFCost = newNeighbourGCost + Vector2Int.Distance(neighbourTileIndex, endPositionIndex);
                    tilesToExplore.Enqueue(neighbour, newNeighbourFCost);
                }
            }
        }
        return null;
    }

    private List<Tile> Reconstruct(Dictionary<Tile, Tile> cameFrom, Tile currentTile)
    {
        List<Tile> result = new List<Tile>();

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
