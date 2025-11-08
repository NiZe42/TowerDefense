using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [SerializeField]
    private Transform buttonContainer;

    [SerializeField]
    private ConfirmationPanel confirmationPanel;

    [SerializeField]
    private SelectionButton sellButton;

    private readonly List<SelectionButton> buildButtons = new List<SelectionButton>();

    private void Awake()
    {
        for (var i = 0; i < buttonContainer.childCount; i++)
        {
            var button = buttonContainer.GetChild(i).GetComponent<SelectionButton>();

            if (button is null)
            {
                Debug.LogError(
                    $"Child {buttonContainer.GetChild(i).name} is missing SelectionButton component!");

                continue;
            }

            buildButtons.Add(button);
        }

        DisableAllButtons();
        gameObject.SetActive(false);
    }

    public void BuildMenu(
        List<TowerLevelDataSO> towerOptions,
        Vector3 menuPosition,
        int id = int.MinValue,
        TowerLevelDataSO towerData = null)
    {
        transform.position = menuPosition;
        DisableAllButtons();

        if (towerOptions != null)
        {
            for (var i = 0; i < towerOptions.Count && i < buildButtons.Count; i++)
            {
                TowerLevelDataSO levelData = towerOptions[i];
                SelectionButton  button    = buildButtons[i];

                SetupBuildButton(
                    button,
                    levelData,
                    menuPosition,
                    id);

                button.gameObject.SetActive(true);
            }
        }

        if (id != int.MinValue)
        {
            ShowSellOption(id, towerData.cumulativeSellValue, menuPosition);
        }

        confirmationPanel.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ShowSellOption(int towerId, int sellValue, Vector3 menuPosition)
    {
        transform.position = menuPosition;

        SetupSellButton(sellButton, towerId, sellValue);
        sellButton.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }

    private void DisableAllButtons()
    {
        foreach (SelectionButton btn in buildButtons)
        {
            btn.gameObject.SetActive(false);
            btn.button.onClick.RemoveAllListeners();
        }

        if (sellButton is null)
        {
            return;
        }

        sellButton.gameObject.SetActive(false);
        sellButton.button.onClick.RemoveAllListeners();
    }

    private void SetupBuildButton(
        SelectionButton button,
        TowerLevelDataSO levelData,
        Vector3 towerPosition,
        int currentTowerId = int.MinValue)
    {
        button.label.text  = levelData.towerName;
        button.icon.sprite = levelData.towerIcon;
        button.button.onClick.AddListener(() =>
        {
            confirmationPanel.ShowBuildConfirmation(levelData, towerPosition, currentTowerId);
        });
    }

    private void SetupSellButton(SelectionButton button, int towerId, int sellValue)
    {
        button.label.text  = "Sell";
        button.icon.sprite = null;
        button.button.onClick.AddListener(() =>
        {
            confirmationPanel.ShowSellConfirmation(towerId, sellValue);
        });
    }
}