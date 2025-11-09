using System;
using UnityEngine;

/// <summary>
///     Data, that defines how many and how big are tiles in <see cref="TileGrid" />>
/// </summary>
[CreateAssetMenu(fileName = "GridPreset", menuName = "GridSystem/GridPreset")]
[Serializable]
public class GridPreset : ScriptableObject
{
    public Vector2Int tileCount;
    public Vector2 gridSize;
}