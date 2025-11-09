using UnityEditor;
using UnityEngine;

/// <summary>
///     A script to allow Editor interaction with <see cref="TileGrid" />>
/// </summary>
[CustomEditor(typeof(TileGrid))]
public class TileGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tileGrid = (TileGrid)target;

        if (GUILayout.Button("Generate tiles"))
        {
            tileGrid.GenerateTiles();
        }

        if (GUILayout.Button("Clear tiles"))
        {
            tileGrid.ClearTiles();
        }
    }
}