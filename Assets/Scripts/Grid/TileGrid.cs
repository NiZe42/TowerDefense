using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class TileGrid : MonoBehaviourSingleton<TileGrid>
{
    [SerializeField]
    internal GridPreset gridPreset;

    public Tile[,] tiles { get; private set; }
    public Vector2 tileSize { get; private set; }

    // TODO: consider this existence
    internal Dictionary<Tower, Block2X2> placedTowers = new Dictionary<Tower, Block2X2>();
    
    static readonly Vector2Int[] PossibleBlockOffsets = {
        new Vector2Int(0, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
    };
    
    static readonly Vector2Int[] NeighboursOffsets = {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
    };

    public override void Awake() {
        base.Awake();
        GenerateTiles();
    }

    public void GenerateTiles()
    {
        tiles = new Tile[gridPreset.tileCount.x, gridPreset.tileCount.y];
        
        tileSize = new Vector3(
            gridPreset.gridSize.x / gridPreset.tileCount.x,
            gridPreset.gridSize.y / gridPreset.tileCount.y
        );

        for (int x = 0; x < gridPreset.tileCount.x; x++)
        {
            for (int y = 0; y < gridPreset.tileCount.y; y++)
            {
                Vector3 worldPosition = transform.position +
                                   new Vector3((x  + .5f) * tileSize.x, 0f, (y + .5f) * tileSize.y);

                tiles[x, y] = new Tile(worldPosition);
            }
        }
    }

    public void ClearTiles() {
        tiles = null;
    }

    public bool TryGetSelectedBlock(Vector2Int selectedTileIndex, Vector3 hitPointWorld, out Block2X2 selectedBlock) {
        selectedBlock = Block2X2.NullBlock2X2();

        // Check if clicked tile already has a tower on it
        if (TryGetTower(selectedTileIndex, out Tower tower)) {
            Debug.Log("Checking out " + tower);
            selectedBlock = placedTowers[tower];
            return true;
        } 

        if (TryGetFreePreferredBlock2X2(selectedTileIndex, hitPointWorld, out selectedBlock)) {
            return true;
        }

        return false;
    }
    
    // Based on hit location and surrounding block tries to return the block that user clicked on.
    // If user clicked on a most top right point on the tile, it should return a block where selected tile is bottom left tile.
    private bool TryGetFreePreferredBlock2X2(Vector2Int index, Vector3 hitPoint, out Block2X2 freePreferredBlock) {
        freePreferredBlock = Block2X2.NullBlock2X2();
        
        if (TryGetAllPossibleBlocks2X2FromTileIndex(index, out List<Block2X2> allPossibleBlocks) == 0) {
            return false;
        }

        float shortestDistance = 100000f;
        foreach (Block2X2 possibleBlock in allPossibleBlocks) {
            float distance = Vector3.Distance(hitPoint, possibleBlock.GetCenterPosition());
            
            if (!(distance < shortestDistance)) continue;
            freePreferredBlock = possibleBlock;
            shortestDistance = distance;
        }
        
        return true;
    }

    private bool TryGetTower(Vector2Int tileIndex, out Tower outTower)
    {
        outTower = null;

        Tower tower = tiles[tileIndex.x, tileIndex.y].currentOccupant;
        if (tower is null)
            return false;

        outTower = tower;
        return true;
    }

    public bool TryGetTileIndexFromWorldPosition(Vector3 worldPos, out Vector2Int tileIndex) {
        tileIndex = default;
        
        Vector3 local = worldPos - transform.position;
        
        int x = Mathf.FloorToInt(local.x / tileSize.x);
        int z = Mathf.FloorToInt(local.z / tileSize.y);
        
        if (x < 0 || z < 0 || x >= gridPreset.tileCount.x || z >= gridPreset.tileCount.y)
            return false;

        tileIndex = new Vector2Int(x, z);
        return true;
    }
    
    public Vector3 GetWorldPositionFromTileIndex(Vector2Int tileIndex)
    {
        float worldX = transform.position.x + tileIndex.x * tileSize.x + tileSize.x * 0.5f;
        float worldZ = transform.position.z + tileIndex.y * tileSize.y + tileSize.y * 0.5f;

        return new Vector3(worldX, transform.position.y, worldZ);
    }
    
    private int TryGetAllPossibleBlocks2X2FromTileIndex(Vector2Int index, out List<Block2X2> blocks) {
        int count = 0;
        
        blocks = new List<Block2X2>();
        
        foreach (Vector2Int offset in PossibleBlockOffsets)
        {
            Vector2Int possibleBottomLeftIndex = index + offset;
            Vector2Int possibleTopRightIndex = possibleBottomLeftIndex + Vector2Int.one;
            
            if (possibleBottomLeftIndex.x < 0 || possibleBottomLeftIndex.y < 0 ||
                possibleBottomLeftIndex.x >= gridPreset.gridSize.x ||
                possibleBottomLeftIndex.y >= gridPreset.gridSize.y)
                continue;

            if (possibleTopRightIndex.x < 0 || possibleTopRightIndex.y < 0 ||
                possibleTopRightIndex.x >= gridPreset.gridSize.x ||
                possibleTopRightIndex.y >= gridPreset.gridSize.y)
                continue;
            
            blocks.Add(new Block2X2(tiles[possibleBottomLeftIndex.x, possibleBottomLeftIndex.y + 1],
                tiles[possibleBottomLeftIndex.x + 1, possibleBottomLeftIndex.y + 1],
                tiles[possibleBottomLeftIndex.x, possibleBottomLeftIndex.y],
                tiles[possibleBottomLeftIndex.x + 1, possibleBottomLeftIndex.y]));
            count++;
        }
        return count;
    }

    public List<Tile> GetNeighbours(Vector2Int tileIndex) {
        List<Tile> neighbours = new List<Tile>();
        foreach (Vector2Int neighbourOffset in NeighboursOffsets) {
            Vector2Int possibleTileIndex = tileIndex + neighbourOffset;

            if (possibleTileIndex.x < 0 ||
                possibleTileIndex.y < 0 ||
                possibleTileIndex.x >= gridPreset.tileCount.x ||
                possibleTileIndex.y >= gridPreset.tileCount.y) {
                continue;
            }
            neighbours.Add(tiles[possibleTileIndex.x, possibleTileIndex.y]);
        }
        return neighbours;
    } 
    
    public bool TryGetTileIndexFromTile(Tile tile, out Vector2Int index)
    {
        index = default;
        
        if (tile == Tile.NullTile() || tiles == null)
            return false;

        for (int x = 0; x < gridPreset.tileCount.x; x++)
        {
            for (int y = 0; y < gridPreset.tileCount.y; y++)
            {
                if (tiles[x, y] == tile)
                {
                    index = new Vector2Int(x, y);
                    return true;
                }
            }
        }
        return false; 
    }
}
