using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
///     Handles Input in the game.
/// </summary>
public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private Camera activeCamera;

    private bool clickQueued;
    private LayerMask mask;
    private LayerMask raycastMask;
    private TileGrid TileGridInstance;

    private void Start()
    {
        activeCamera     = Camera.main;
        TileGridInstance = TileGrid.Instance;
        raycastMask      = LayerMask.GetMask("TilePlane", "Tower");
    }

    public void Update()
    {
        if (!clickQueued)
        {
            return;
        }

        Debug.Log("OnLeftMouseClicked");
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked UI, ignore world click");
            clickQueued = false;
            return;
        }

        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f,
            raycastMask))
        {
            ProcessRaycastHit(hit);
            clickQueued = false;
            return;
        }

        EventBus.Instance.InvokeEvent(new OnNothingSelected());
        clickQueued = false;
    }

    public void OnLeftMouseClicked()
    {
        clickQueued = true;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ProcessRaycastHit(RaycastHit hit)
    {
        int layer = hit.collider.gameObject.layer;

        if (layer == LayerMask.NameToLayer("TilePlane"))
        {
            HandleTile(hit.point);
            return;
        }

        if (layer == LayerMask.NameToLayer("Tower"))
        {
            HandleTower(hit.collider.gameObject);
        }
    }

    private void HandleTile(Vector3 hitPoint)
    {
        Debug.Log("OnMousePress");

        if (!TileGridInstance.TryGetTileIndexFromWorldPosition(
            hitPoint,
            out Vector2Int selectedTileIndex))
        {
            return;
        }

        if (!TileGridInstance.TryGetSelectedBlock(
            selectedTileIndex,
            hitPoint,
            out Block2X2 possibleSelectedBlock))
        {
            EventBus.Instance.InvokeEvent(new OnNothingSelected());
            return;
        }

        EventBus.Instance.InvokeEvent(
            new OnFreeBlock2X2Selected { blockCenter = possibleSelectedBlock.GetCenterPosition() });
    }

    private void HandleTower(GameObject towerObject)
    {
        Tower tower = GetRootTowerScript(towerObject);

        if (tower == null)
        {
            Debug.Log("Tower not found in Tower - should not happen.");
            return;
        }

        EventBus.Instance.InvokeEvent(new OnTowerSelected { towerId = tower.id });
    }

    private Tower GetRootTowerScript(GameObject towerChild)
    {
        Transform currentTransform = towerChild.transform;

        while (currentTransform is not null)
        {
            var tower = currentTransform.GetComponent<Tower>();
            if (tower != null)
            {
                return tower;
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }

    public void IncreaseTimeMultiplier()
    {
        Debug.Log("IncreaseTimeMultiplier");
        GameManager.Instance.IncrementTimeMultiplier(true);
    }

    public void DecreaseTimeMultiplier()
    {
        Debug.Log("DecreaseTimeMultiplier");
        GameManager.Instance.IncrementTimeMultiplier(false);
    }
}