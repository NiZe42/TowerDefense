using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
///     Manager-type class that provides access to most of UI in the game.
/// </summary>
public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField]
    private GameObject selectionPrefab;

    [SerializeField]
    private GameObject radialMenuPrefab;

    [SerializeField]
    private Canvas UICanvas;

    [SerializeField]
    private PlayerHealthUI playerHealthUI;

    [SerializeField]
    private GameObject floatingMoneyPrefab;

    [SerializeField]
    private PlayerMoneyUI playerMoneyUI;

    [SerializeField]
    private GameStateUI gameStateUI;

    [SerializeField]
    private TextMeshProUGUI endGameResult;

    private RadialMenu currentRadialMenu;

    private GameObject currentSelection;

    public IEconomyValidator economyValidator;

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
        EventBus.Instance.Subscribe<OnEnemyDestroyed>(ShowFloatingMoney);
    }

    public override void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<OnFreeBlock2X2Selected>((Action<IEvent>)ProcessSelection);
            EventBus.Instance.Unsubscribe<OnTowerSelected>((Action<IEvent>)ProcessSelection);
            EventBus.Instance.Unsubscribe<OnNothingSelected>((Action<IEvent>)ProcessSelection);
            EventBus.Instance.Unsubscribe<OnEnemyDestroyed>(ShowFloatingMoney);
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

    public void ShowFloatingMoney(OnEnemyDestroyed @event)
    {
        if (floatingMoneyPrefab == null)
        {
            Debug.LogWarning("FloatingMoneyPrefab not assigned!");
            return;
        }

        GameObject floatingMoney = Instantiate(
            floatingMoneyPrefab,
            @event.deathPosition + Vector3.up * 2f,
            Quaternion.identity);

        var floatingText = floatingMoney.GetComponent<FloatingText>();
        floatingText.Initialize($"+{@event.droppedMoney}", Color.yellow);
    }

    public void ClearSelection()
    {
        currentRadialMenu.gameObject.SetActive(false);
        currentSelection.SetActive(false);
        isSelecting = false;
    }

    public PlayerHealthUI GetPlayerHealthUI()
    {
        return playerHealthUI;
    }

    public PlayerMoneyUI GetPlayerMoneyUI()
    {
        return playerMoneyUI;
    }

    public GameStateUI GetGameStateUI()
    {
        return gameStateUI;
    }

    public void TriggerEndGame(bool hasWon)
    {
        if (hasWon)
        {
            endGameResult.text = "You have won!\nPress R to restart.";
        }
        else
        {
            endGameResult.text = "You have lost!\nPress R to restart.";
        }

        endGameResult.gameObject.SetActive(true);
    }
}