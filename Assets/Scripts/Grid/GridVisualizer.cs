using System;
using UnityEngine;
[RequireComponent(typeof(TileGrid))]
[RequireComponent(typeof(GridSelection))]
public class GridVisualizer : MonoBehaviour {
    
    [SerializeField]
    private float tileHeightOffset;
    [SerializeField]
    private GridSelection gridSelection;
    [SerializeField]
    private TileGrid tileGrid;

    private void Start() {
        
    }

    private void OnDrawGizmos() {
        if (tileGrid?.tiles == null)
            return;
        
        Vector2 tileSize = tileGrid.tileSize;
        
        for (int x = 0; x < tileGrid.gridPreset.tileCount.x; x++) {
            for (int y = 0; y < tileGrid.gridPreset.tileCount.y; y++) {
                Tile tile = tileGrid.tiles[x, y];
                Vector3 center = tile.worldPosition + new Vector3(0f, tileHeightOffset, 0f);
                
                Gizmos.color = tile.IsOccupied() ? Color.red : Color.black;
                
                Gizmos.DrawWireCube(center, new Vector3(tileSize.x, 0.01f, tileSize.y));
            }

            foreach (Tile tile in gridSelection.selectedBlock.GetAllTiles()) {
                
                Gizmos.color = Color.yellow;

                float outlineScale = 1f; 
                Vector3 center = tile.worldPosition + new Vector3(0f, tileHeightOffset, 0f);
                Gizmos.DrawWireCube(center,
                    new Vector3(tileSize.x * outlineScale, 0.02f, tileSize.y * outlineScale));
            }
        }
    }
}
