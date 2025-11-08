using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField]
    private GameObject selectionPrefab;

    [SerializeField]
    private GameObject radialMenuPrefab;

    [SerializeField]
    private Canvas UICanvas;

    private RadialMenu currentRadialMenu;

    private GameObject currentSelection;

    private bool isSelecting;

    public override void Awake()
    {
        base.Awake();
        currentSelection = Instantiate(selectionPrefab, Vector3.zero, Quaternion.identity);
        isSelecting      = false;
        currentSelection.SetActive(false);

        currentRadialMenu = Instantiate(
            radialMenuPrefab,
            Vector3.zero,
            Quaternion.identity,
            UICanvas.transform).GetComponent<RadialMenu>();

        currentRadialMenu.gameObject.SetActive(false);
    }

    private void Start()
    {
        EventBus.Instance.Subscribe<OnFreeBlock2X2Selected>((Action<IEvent>)ProcessSelection);
        EventBus.Instance.Subscribe<OnTowerSelected>((Action<IEvent>)ProcessSelection);
        EventBus.Instance.Subscribe<OnNothingSelected>((Action<IEvent>)ProcessSelection);
    }

    public override void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<OnFreeBlock2X2Selected>((Action<IEvent>)ProcessSelection);
            EventBus.Instance.Unsubscribe<OnTowerSelected>((Action<IEvent>)ProcessSelection);
            EventBus.Instance.Unsubscribe<OnNothingSelected>((Action<IEvent>)ProcessSelection);
        }

        base.OnDestroy();
    }

    private void ProcessSelection(IEvent @event)
    {
        Vector3 spawnPosition;
        switch (@event)
        {
            case OnTowerSelected towerSelected:
                Tower tower = TowerManager.Instance.towers[towerSelected.towerId];
                spawnPosition = tower.transform.position;

                List<TowerLevelDataSO> upgradeOptions = tower.GetUpgradeOptions();

                currentRadialMenu.BuildMenu(
                    upgradeOptions,
                    spawnPosition,
                    tower.id,
                    tower.GetTowerData());

                break;

            case OnFreeBlock2X2Selected blockSelected:
                spawnPosition = blockSelected.blockCenter;

                List<TowerLevelDataSO> buildOptions = TowerManager.Instance.towerUpgradeDatabase
                    .GetBasicTowers().ToList();

                currentRadialMenu.BuildMenu(buildOptions, spawnPosition);

                break;

            case OnNothingSelected nothingSelected:
                isSelecting = false;
                currentSelection.SetActive(false);
                currentRadialMenu.gameObject.SetActive(false);
                return;

            default:
                Debug.Log($"Unknown selection event type: {@event.GetType()}");
                return;
        }

        currentSelection.transform.position = spawnPosition;
        currentSelection.SetActive(true);

        MoveRadialMenuToWorldPosition(spawnPosition);

        isSelecting = true;
    }

    private void MoveRadialMenuToWorldPosition(Vector3 worldPosition)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)UICanvas.transform,
            screenPoint,
            null,
            out Vector2 uiPosition);

        currentRadialMenu.GetComponent<RectTransform>().anchoredPosition = uiPosition;
    }

    public void ClearSelection()
    {
        currentRadialMenu.gameObject.SetActive(false);
        currentSelection.SetActive(false);
        isSelecting = false;
    }
}