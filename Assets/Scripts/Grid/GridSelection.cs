using UnityEngine;

[RequireComponent(typeof(TileGrid))]
public class GridSelection : MonoBehaviour
{
    [SerializeField]
    private Camera activeCamera;
    
    [SerializeField]
    private TileGrid tileGrid;

    [SerializeField]
    private LayerMask gridPlaneMask;
    
    public Block2X2 selectedBlock =  Block2X2.NullBlock2X2();
    
    private void Start() {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMousePress();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    void OnMousePress() {
        
        Debug.Log("OnMousePress");
        
        if (!RaycastToTile(out Vector2 hitPosition))
            return;

        if (!tileGrid.TryGetTileIndexFromWorldPosition(hitPosition, out Vector2Int selectedTileIndex)) {
            return;
        }

        if (!tileGrid.TryGetSelectedBlock(selectedTileIndex, hitPosition, out Block2X2 possibleSelectedBlock)) {
            EventBus.Instance.InvokeEvent(new NoPossibleBoxFoundAfterClick());
            selectedBlock = Block2X2.NullBlock2X2();
            return;
        }

        if (possibleSelectedBlock.topLeftTile.IsOccupied()) {
            EventBus.Instance.InvokeEvent(new TowerSelected{towerCenter = possibleSelectedBlock.GetCenterPosition()});
            selectedBlock = possibleSelectedBlock;
            return;
        }
        
        EventBus.Instance.InvokeEvent(new FreeBlock2X2Selected{blockCenter = possibleSelectedBlock.GetCenterPosition()});

        Debug.Log("Creating a tower");
        
        Tile[] tiles = possibleSelectedBlock.GetAllTiles();
        Tower tower = new Tower(); 
        for(int i  = 0; i < tiles.Length; i++)
        {
            tiles[i].currentOccupant = tower;
            
            if (tileGrid.TryGetTileIndexFromTile(tiles[i], out Vector2Int tileIndex)) {
                Debug.Log("placing tower into: " + tileIndex);
                Debug.Log(tileGrid.tiles[tileIndex.x, tileIndex.y].currentOccupant);
            }
        }
        
        
        
        tileGrid.placedTowers[tower] = possibleSelectedBlock;
        Debug.Log(tileGrid.placedTowers[tower]);
        selectedBlock = possibleSelectedBlock;
    }
    
    bool RaycastToTile(out Vector2 raycastHitPosition) {
        raycastHitPosition = default;

        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 10000, gridPlaneMask))
            return false;

        raycastHitPosition = new Vector2(hit.point.x, hit.point.z);
        return true;
    }
}
