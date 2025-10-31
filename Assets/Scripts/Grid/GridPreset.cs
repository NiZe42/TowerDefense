using UnityEngine;

[CreateAssetMenu(fileName = "GridPreset", menuName = "GridSystem/GridPreset")]
[System.Serializable]
public class GridPreset : ScriptableObject
{
    public Vector2Int tileCount;  
    public Vector2 gridSize;
}
