using UnityEngine;

/// <summary>
///     Basic Block of selection and Tower building.
///     All towers are standing on a 2x2 tile.
/// </summary>
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