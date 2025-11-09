using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Visualizes the path calculated by <see cref="PathFindingManager" /> on the tile grid.
///     Updates tile quad materials to show  path, start, and end positions.
/// </summary>
public class PathVisualizer : MonoBehaviour
{
    [SerializeField]
    private float tileHeightOffset;

    [SerializeField]
    private Material pathMaterial;

    [SerializeField]
    private Material startMaterial;

    [SerializeField]
    private Material endMaterial;

    [SerializeField]
    private Material defaultLightMaterial;

    [SerializeField]
    private Material defaultDarkMaterial;

    private TileGrid tileGrid;
    private void Start() { }

    private void OnEnable()
    {
        PathFindingManager.OnPathUpdated += PathFindingManager_OnPathUpdated;
    }

    private void OnDisable()
    {
        if (PathFindingManager.OnPathUpdated != null)
        {
            PathFindingManager.OnPathUpdated -= PathFindingManager_OnPathUpdated;
        }
    }

    public void Initialize(TileGrid tileGrid, List<Tile> path)
    {
        this.tileGrid = tileGrid;
        PaintPathTiles(path);
    }

    /*
        public void OnDrawGizmos()
        {
            if (pathFindingManager.currentPath == null)
            {
                return;
            }

            path = pathFindingManager.currentPath;

            Gizmos.color = Color.green;

            for (var i = 0; i < path.Count; i++)
            {
                Tile    tile     = path[i];
                Vector3 worldPos = tile.worldPosition + new Vector3(0f, tileHeightOffset, 0f);

                if (i == 0)
                {
                    Gizmos.color = Color.blue;
                }

                if (i == path.Count - 1)
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawCube(worldPos, new Vector3(0.2f, 0.2f, 0.2f));

                Gizmos.color = Color.green;
                if (i < path.Count - 1)
                {
                    Tile    nextTile = path[i + 1];
                    Vector3 nextPos  = nextTile.worldPosition + new Vector3(0f, tileHeightOffset, 0f);
                    Gizmos.DrawLine(worldPos, nextPos);
                }
            }
        }*/

    private void PathFindingManager_OnPathUpdated(List<Tile> enemiesPath)
    {
        ClearPathVisual();

        PaintPathTiles(enemiesPath);
    }

    private void PaintPathTiles(List<Tile> pathTiles)
    {
        for (var i = 0; i < pathTiles.Count; i++)
        {
            Tile tile = pathTiles[i];

            if (tile.quad == null)
            {
                Debug.Log("lala");
                continue;
            }

            if (i == 0)
            {
                tile.quad.GetTileMeshRenderer().material = startMaterial;
            }
            else if (i == pathTiles.Count - 1)
            {
                tile.quad.GetTileMeshRenderer().material = endMaterial;
            }
            else
            {
                tile.quad.GetTileMeshRenderer().material = pathMaterial;
            }
        }
    }

    private void ClearPathVisual()
    {
        if (tileGrid.tiles == null)
        {
            return;
        }

        for (var x = 0; x < tileGrid.gridPreset.tileCount.x; x++)
        {
            for (var y = 0; y < tileGrid.gridPreset.tileCount.y; y++)
            {
                Tile tile = tileGrid.tiles[x, y];

                if (tile.quad == null)
                {
                    continue;
                }

                tile.quad.GetTileMeshRenderer().material =
                    (x + y) % 2 == 0 ? defaultLightMaterial : defaultDarkMaterial;
            }
        }
    }
}