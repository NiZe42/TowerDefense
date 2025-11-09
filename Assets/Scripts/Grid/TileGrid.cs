using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
///     Grid on which everything is happening.
///     Handles queries about tiles, world positions or indexes.
/// </summary>
[RequireComponent(typeof(GridBuilder))]
public class TileGrid : MonoBehaviourSingleton<TileGrid>
{
    private static readonly Vector2Int[] PossibleBlockOffsets =
    {
        new Vector2Int(0, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1)
    };

    private static readonly Vector2Int[] NeighboursOffsets =
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1)
    };

    public GridPreset gridPreset;

    [SerializeField]
    private bool isDebug;

    [SerializeField]
    private GridBuilder gridBuilder;

    public IPathValidator pathValidator;

    public Tile[,] tiles { get; private set; }
    public Vector2 tileSize { get; private set; }

    public override void Awake()
    {
        base.Awake();
        GenerateTiles();
        gridBuilder.Initialize(this);
    }

    public void Start() { }

    public void GenerateTiles()
    {
        tiles = new Tile[gridPreset.tileCount.x, gridPreset.tileCount.y];

        tileSize = new Vector3(
            gridPreset.gridSize.x / gridPreset.tileCount.x,
            gridPreset.gridSize.y / gridPreset.tileCount.y);

        for (var x = 0; x < gridPreset.tileCount.x; x++)
        {
            for (var y = 0; y < gridPreset.tileCount.y; y++)
            {
                Vector3 worldPosition = transform.position + new Vector3(
                    (x + .5f) * tileSize.x,
                    0f,
                    (y + .5f) * tileSize.y);

                tiles[x, y] = new Tile(worldPosition);
            }
        }

        if (isDebug)
        {
            tiles[1, 1].isOccupied = true;
            tiles[1, 2].isOccupied = true;
            tiles[2, 1].isOccupied = true;
            tiles[2, 2].isOccupied = true;

            tiles[1, 3].isOccupied = true;
            tiles[2, 3].isOccupied = true;
            tiles[1, 4].isOccupied = true;
            tiles[2, 4].isOccupied = true;

            tiles[3, 1].isOccupied = true;
            tiles[3, 2].isOccupied = true;
            tiles[4, 1].isOccupied = true;
            tiles[4, 2].isOccupied = true;
        }
    }

    public void ClearTiles()
    {
        tiles = null;
    }

    public bool TryGetSelectedBlock(
        Vector2Int selectedTileIndex,
        Vector3 hitPointWorld,
        out Block2X2 selectedBlock)
    {
        selectedBlock = Block2X2.NullBlock2X2();

        return TryGetFreePreferredBlock2X2(selectedTileIndex, hitPointWorld, out selectedBlock);
    }

    // Based on hit location and surrounding block tries to return the block that user clicked on.
    // If user clicked on a most top right point on the tile, it should return a block where selected tile is bottom left tile.
    private bool TryGetFreePreferredBlock2X2(
        Vector2Int index,
        Vector3 hitPoint,
        out Block2X2 freePreferredBlock)
    {
        freePreferredBlock = Block2X2.NullBlock2X2();

        if (TryGetAllPossibleBlocks2X2FromTileIndex(index, out List<Block2X2> allPossibleBlocks) ==
            0)
        {
            return false;
        }

        var shortestDistance = 100000f;
        foreach (Block2X2 possibleBlock in allPossibleBlocks)
        {
            float distance = Vector3.Distance(hitPoint, possibleBlock.GetCenterPosition());

            if (!(distance < shortestDistance))
            {
                continue;
            }

            if (!pathValidator.IsPathStillValid(possibleBlock.GetCenterPosition()))
            {
                continue;
            }

            freePreferredBlock = possibleBlock;
            shortestDistance   = distance;
        }

        return freePreferredBlock.bottomLeftTile != Tile.NullTile();
    }

    public bool TryGetTileIndexFromWorldPosition(Vector3 worldPos, out Vector2Int tileIndex)
    {
        tileIndex = default;

        Vector3 local = worldPos - transform.position;

        int x = Mathf.FloorToInt(local.x / tileSize.x);
        int z = Mathf.FloorToInt(local.z / tileSize.y);

        if (x < 0 || z < 0 || x >= gridPreset.tileCount.x || z >= gridPreset.tileCount.y)
        {
            return false;
        }

        tileIndex = new Vector2Int(x, z);
        return true;
    }

    public Vector3 GetWorldPositionFromTileIndex(Vector2Int tileIndex)
    {
        float worldX = transform.position.x + tileIndex.x * tileSize.x + tileSize.x * 0.5f;
        float worldZ = transform.position.z + tileIndex.y * tileSize.y + tileSize.y * 0.5f;

        return new Vector3(worldX, transform.position.y, worldZ);
    }

    public bool TryGetTileIndexesFromBlockCenterPosition(
        Vector3 worldPos,
        out Vector2Int[] tileIndexes)
    {
        tileIndexes = new Vector2Int[] { };

        var topRightWorld = new Vector3(
            worldPos.x + tileSize.x * 0.5f,
            0f,
            worldPos.z + tileSize.y * 0.5f);

        if (!TryGetTileIndexFromWorldPosition(topRightWorld, out Vector2Int topRight))
        {
            return false;
        }

        tileIndexes = new[]
        {
            topRight,
            new Vector2Int(topRight.x, topRight.y - 1),
            new Vector2Int(topRight.x - 1, topRight.y),
            new Vector2Int(topRight.x - 1, topRight.y - 1)
        };

        return true;
    }

    private int TryGetAllPossibleBlocks2X2FromTileIndex(Vector2Int index, out List<Block2X2> blocks)
    {
        var count = 0;

        blocks = new List<Block2X2>();

        foreach (Vector2Int offset in PossibleBlockOffsets)
        {
            Vector2Int possibleBottomLeftIndex  = index + offset;
            Vector2Int possibleBottomRightIndex = possibleBottomLeftIndex + Vector2Int.up;
            Vector2Int possibleTopRightIndex    = possibleBottomLeftIndex + Vector2Int.one;
            Vector2Int possibleTopLeftIndex     = possibleBottomLeftIndex + Vector2Int.right;

            if (possibleBottomLeftIndex.x < 0 || possibleBottomLeftIndex.y < 0 ||
                possibleBottomLeftIndex.x >= gridPreset.gridSize.x ||
                possibleBottomLeftIndex.y >= gridPreset.gridSize.y)
            {
                continue;
            }

            if (possibleTopRightIndex.x < 0 || possibleTopRightIndex.y < 0 ||
                possibleTopRightIndex.x >= gridPreset.gridSize.x ||
                possibleTopRightIndex.y >= gridPreset.gridSize.y)
            {
                continue;
            }

            if (tiles[possibleTopLeftIndex.x, possibleTopLeftIndex.y].isOccupied ||
                tiles[possibleTopRightIndex.x, possibleTopRightIndex.y].isOccupied ||
                tiles[possibleBottomLeftIndex.x, possibleBottomLeftIndex.y].isOccupied ||
                tiles[possibleBottomRightIndex.x, possibleBottomRightIndex.y].isOccupied)
            {
                continue;
            }

            blocks.Add(
                new Block2X2(
                    tiles[possibleTopLeftIndex.x, possibleTopLeftIndex.y],
                    tiles[possibleTopRightIndex.x, possibleTopRightIndex.y],
                    tiles[possibleBottomLeftIndex.x, possibleBottomLeftIndex.y],
                    tiles[possibleBottomRightIndex.x, possibleBottomRightIndex.y]));

            count++;
        }

        return count;
    }

    public List<Tile> GetNeighbours(Vector2Int tileIndex)
    {
        var neighbours = new List<Tile>();
        foreach (Vector2Int neighbourOffset in NeighboursOffsets)
        {
            Vector2Int possibleTileIndex = tileIndex + neighbourOffset;

            if (possibleTileIndex.x < 0 || possibleTileIndex.y < 0 ||
                possibleTileIndex.x >= gridPreset.tileCount.x ||
                possibleTileIndex.y >= gridPreset.tileCount.y)
            {
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
        {
            return false;
        }

        for (var x = 0; x < gridPreset.tileCount.x; x++)
        {
            for (var y = 0; y < gridPreset.tileCount.y; y++)
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

    public void ChangeBlockOccupancy(Vector3 blockPosition, bool newOccupancy)
    {
        if (!TryGetTileIndexesFromBlockCenterPosition(blockPosition, out Vector2Int[] tileIndexes))
        {
            return;
        }

        foreach (Vector2Int index in tileIndexes)
        {
            tiles[index.x, index.y].isOccupied = newOccupancy;
        }
    }
}