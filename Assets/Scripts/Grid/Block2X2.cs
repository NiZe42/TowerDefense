using UnityEngine;

// All towers in the game are designed to be 2x2 so there is no reason to select anything smaller than that.
// Main manipulations are with bottom left, as it is closer to start of coords.

public struct Block2X2
{
    public Tile topLeftTile;
    public Tile topRightTile;
    public Tile bottomLeftTile;
    public Tile bottomRightTile;

    private static readonly Block2X2 nullBlock2X2 = new Block2X2(
        Tile.NullTile(),
        Tile.NullTile(),
        Tile.NullTile(),
        Tile.NullTile());

    public Block2X2(
        Tile topLeftTile,
        Tile topRightTile,
        Tile bottomLeftTile,
        Tile bottomRightTile)
    {
        this.topLeftTile     = topLeftTile;
        this.topRightTile    = topRightTile;
        this.bottomLeftTile  = bottomLeftTile;
        this.bottomRightTile = bottomRightTile;
    }

    public static Block2X2 NullBlock2X2()
    {
        return nullBlock2X2;
    }

    public Tile[] GetAllTiles()
    {
        return new[]
        {
            topLeftTile,
            topRightTile,
            bottomLeftTile,
            bottomRightTile
        };
    }

    public Vector3 GetCenterPosition()
    {
        Vector3 allPositionsSum = topLeftTile.worldPosition + topRightTile.worldPosition +
            bottomLeftTile.worldPosition + bottomRightTile.worldPosition;

        return allPositionsSum / 4;
    }

    public bool IsTilePartOfThis(Tile tile)
    {
        return topLeftTile == tile || topRightTile == tile || bottomLeftTile == tile ||
            bottomRightTile == tile;
    }
}