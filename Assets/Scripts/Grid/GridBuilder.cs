using UnityEngine;

/// <summary>
///     Visualizes Grid from <see cref="TileGrid" />>.
///     Adding quads and painting them in checkerboard formation.
/// </summary>
public class GridBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject quadPrefab;

    [SerializeField]
    private Material lightMaterial;

    [SerializeField]
    private Material darkMaterial;

    private TileGrid tileGrid;

    internal void Initialize(TileGrid tileGrid)
    {
        this.tileGrid = tileGrid;
        BuildVisualGrid();
    }

    private void BuildVisualGrid()
    {
        if (tileGrid.tiles == null)
        {
            return;
        }

        Vector2 tileSize = tileGrid.tileSize;

        for (var x = 0; x < tileGrid.gridPreset.tileCount.x; x++)
        {
            for (var y = 0; y < tileGrid.gridPreset.tileCount.y; y++)
            {
                Vector3 worldPos = tileGrid.tiles[x, y].worldPosition;

                GameObject quad = Instantiate(
                    quadPrefab,
                    new Vector3(worldPos.x, tileGrid.transform.position.y + .1f, worldPos.z),
                    Quaternion.Euler(90f, 0f, 0f),
                    transform);

                tileGrid.tiles[x, y].quad = quad.GetComponent<TileQuad>();
                Debug.Log(tileGrid.tiles[x, y].quad);
                quad.transform.localScale = new Vector3(
                    tileSize.x / tileGrid.tiles.GetLength(0),
                    tileSize.y / tileGrid.tiles.GetLength(1),
                    1f);

                quad.GetComponent<MeshRenderer>().material =
                    (x + y) % 2 == 0 ? lightMaterial : darkMaterial;
            }
        }
    }
}