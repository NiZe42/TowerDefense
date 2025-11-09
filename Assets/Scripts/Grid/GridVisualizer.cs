using UnityEngine;

/// <summary>
///     Old version of visualizer for <see cref="TileGrid" />.
///     Uses OnDrawGizmos().
/// </summary>
[RequireComponent(typeof(TileGrid))]
public class GridVisualizer : MonoBehaviour
{
    [SerializeField]
    private float tileHeightOffset;

    [SerializeField]
    private TileGrid tileGrid;

    private void Start() { }

    private void OnDrawGizmos()
    {
        if (tileGrid?.tiles == null)
        {
            return;
        }

        Vector2 tileSize = tileGrid.tileSize;

        for (var x = 0; x < tileGrid.gridPreset.tileCount.x; x++)
        {
            for (var y = 0; y < tileGrid.gridPreset.tileCount.y; y++)
            {
                Tile    tile   = tileGrid.tiles[x, y];
                Vector3 center = tile.worldPosition + new Vector3(0f, tileHeightOffset, 0f);

                Gizmos.color = tile.isOccupied ? Color.red : Color.black;

                Gizmos.DrawWireCube(center, new Vector3(tileSize.x, 0.01f, tileSize.y));
            }
        }
    }
}