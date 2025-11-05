using System;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField]
    private GameObject selectionPrefab;

    private GameObject currentSelection;

    private bool isSelecting;

    public override void Awake()
    {
        base.Awake();
        currentSelection = Instantiate(selectionPrefab, Vector3.zero, Quaternion.identity);
        isSelecting      = false;
        currentSelection.SetActive(false);
    }

    private void Start()
    {
        EventBus.Instance.Subscribe<OnFreeBlock2X2Selected>((Action<IEvent>)ProcessSelection);
        EventBus.Instance.Subscribe<OnTowerSelected>((Action<IEvent>)ProcessSelection);
        EventBus.Instance.Subscribe<OnNothingSelected>((Action<IEvent>)ProcessSelection);
    }

    public override void OnDestroy()
    {
        if (EventBus.Instance is null)
        {
            return;
        }

        EventBus.Instance.Unsubscribe<OnFreeBlock2X2Selected>((Action<IEvent>)ProcessSelection);
        EventBus.Instance.Unsubscribe<OnTowerSelected>((Action<IEvent>)ProcessSelection);
        EventBus.Instance.Unsubscribe<OnNothingSelected>((Action<IEvent>)ProcessSelection);
    }

    private void ProcessSelection(IEvent @event)
    {
        Vector3 spawnPosition;
        switch (@event)
        {
            case OnTowerSelected towerSelected:
                spawnPosition = TowerManager.Instance.towers[towerSelected.towerId].transform
                    .position;

                break;

            case OnFreeBlock2X2Selected blockSelected:
                spawnPosition = blockSelected.blockCenter;

                break;

            case OnNothingSelected nothingSelected:
                isSelecting = false;
                currentSelection.SetActive(false);
                return;

            default:
                Debug.Log($"Unknown selection event type: {@event.GetType()}");
                return;
        }

        currentSelection.transform.position = spawnPosition;
        currentSelection.SetActive(true);
        isSelecting = true;
    }
}