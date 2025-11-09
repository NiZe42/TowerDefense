using UnityEngine;

/// <summary>
///     Quad that is spawned above grid plane to visualize tiles.
/// </summary>
public class TileQuad : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer quad;

    private void Awake() { }

    public MeshRenderer GetTileMeshRenderer()
    {
        return quad;
    }
}