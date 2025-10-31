using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFindingManager))]
public class PathVisualizer : MonoBehaviour
{
    [SerializeField]
    private float tileHeightOffset;
    [SerializeField]
    PathFindingManager pathFindingManager;
    
    private Vector3 gridWorldPosition;
    private List<Tile> path = new List<Tile>();
    private void Start() {
        
    }

    private void OnEnable() {
        PathFindingManager.OnPathUpdated += PathFindingManager_OnPathUpdated;
    }

    private void OnDisable() {
        PathFindingManager.OnPathUpdated -= PathFindingManager_OnPathUpdated;
    }

    public void OnDrawGizmos() {
        if (pathFindingManager.currentPath == null) {
            return;
        }

        path = pathFindingManager.currentPath;

        Gizmos.color = Color.green;

        for (int i = 0; i < path.Count; i++)
        {
            Tile tile = path[i];
            Vector3 worldPos = new Vector3(tile.worldPosition.x, gridWorldPosition.y + tileHeightOffset, tile.worldPosition.y);
            
            if(i == 0) Gizmos.color = Color.blue;
            if(i == path.Count - 1) Gizmos.color = Color.red;
            Gizmos.DrawCube(worldPos, new Vector3(0.2f, 0.2f, 0.2f));
            
            Gizmos.color = Color.green;
            if (i < path.Count - 1)
            {
                Tile nextTile = path[i + 1];
                Vector3 nextPos = new Vector3(nextTile.worldPosition.x, gridWorldPosition.y + tileHeightOffset, nextTile.worldPosition.y);
                Gizmos.DrawLine(worldPos, nextPos);
            }
        }
    }

    void PathFindingManager_OnPathUpdated(List<Tile> enemiesPath) {
        this.path = enemiesPath;
        gridWorldPosition = pathFindingManager.tileGrid.gameObject.transform.position;
    }
}
