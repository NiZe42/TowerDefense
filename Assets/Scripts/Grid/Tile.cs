using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : IEquatable<Tile>
{
    public Vector2 worldPosition;
    public Tower currentOccupant;

    private static readonly Tile nullTile = new Tile(new Vector2(float.MinValue, float.MaxValue));
    
    // Tiles are never meant to be one above the other.
    // So we need to only compare position to get if those are the same tiles or not.
    public Tile(Vector2 position, Tower currentOccupant = null) {
        this.worldPosition = position;
        this.currentOccupant = currentOccupant;
    }

    public static Tile NullTile() {
        return nullTile;
    }

    public bool Equals(Tile otherTile) {
        return worldPosition == otherTile.worldPosition;
    }
    
    public override bool Equals(object obj) {
        if (obj is Tile otherTile)
        {
            return Equals(otherTile); 
        }
        return false;
    }

    public override int GetHashCode() {
        return worldPosition.GetHashCode();
    }

    public static bool operator ==(Tile tile1, Tile tile2)
    {
        return tile1.Equals(tile2);
    }
    
    public static bool operator !=(Tile tile1, Tile tile2)
    {
        return !tile1.Equals(tile2);
    }

    public bool IsOccupied() {
        return currentOccupant is not null;
    }
}
