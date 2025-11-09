using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Panel, that pops up after you have clicked on <see cref="SelectionButton" />> to build/upgrade/sell.
/// </summary>
public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private TextMeshProUGUI statsText;

    private ICommand pendingCommand;

    private void Start()
    {
        confirmButton.onClick.AddListener(() =>
        {
            pendingCommand.Execute();
            pendingCommand = null;
            gameObject.SetActive(false);
            UIManager.Instance.ClearSelection();
        });

        cancelButton.onClick.AddListener(() =>
        {
            pendingCommand = null;
            gameObject.SetActive(false);
        });
    }

    public void ShowBuildConfirmation(
        TowerLevelDataSO nextLevelData,
        Vector3 selectionPosition,
        int currentTowerId = int.MinValue)
    {
        statsText.text = $"{nextLevelData.towerName}\nDamage: {nextLevelData.damage}\n" +
            $"Range: {nextLevelData.range}\nAttack Cooldown: {nextLevelData.attackCooldown}\n" +
            $"Attack Type: {nextLevelData.shootingType.ToString()}";

        if (currentTowerId != int.MinValue)
        {
            pendingCommand = new UpgradeTowerCommand(currentTowerId, nextLevelData);
        }
        else
        {
            pendingCommand = new BuildTowerCommand(selectionPosition, nextLevelData);
        }

        gameObject.SetActive(true);
    }

    public void ShowSellConfirmation(int towerId, int sellValue)
    {
        statsText.text = $"Sell for {sellValue} gold?";
        pendingCommand = new SellTowerCommand(towerId);
        gameObject.SetActive(true);
    }
}