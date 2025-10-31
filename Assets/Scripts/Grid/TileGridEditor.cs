using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileGrid))]
public class TileGridEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileGrid tileGrid = (TileGrid)target;
        
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