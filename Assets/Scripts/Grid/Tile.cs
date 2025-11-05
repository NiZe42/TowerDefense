using System;
using UnityEngine;

public class Tile : IEquatable<Tile>
{
    private static readonly Tile nullTile =
        new Tile(new Vector3(float.MinValue, float.MaxValue, float.MinValue));

    public bool isOccupied;
    public Vector3 worldPosition;

    public Tile(Vector3 position, bool isOccupied = false)
    {
        worldPosition   = position;
        this.isOccupied = isOccupied;
    }

    public bool Equals(Tile otherTile)
    {
        return worldPosition == otherTile?.worldPosition && isOccupied == otherTile.isOccupied;
        ;
    }

    public static Tile NullTile()
    {
        return nullTile;
    }

    public override bool Equals(object obj)
    {
        if (obj is Tile otherTile)
        {
            return Equals(otherTile);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return worldPosition.GetHashCode();
    }

    public static bool operator==(Tile tile1, Tile tile2)
    {
        return tile1.Equals(tile2);
    }

    public static bool operator!=(Tile tile1, Tile tile2)
    {
        return!tile1.Equals(tile2);
    }
}