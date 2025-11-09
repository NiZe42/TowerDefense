using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Button responsible for options in <see cref="RadialMenu" />>.
///     Can shows sell or build options.
/// </summary>
public class SelectionButton : MonoBehaviour
{
    [HideInInspector]
    public TowerLevelDataSO levelData;

    public Button button;
    public TextMeshProUGUI label;
    public TextMeshProUGUI price;
    public Image icon;

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;

        Color textColor = interactable
            ? Color.black
            : new Color(
                0.8f,
                0.8f,
                0.8f,
                0.6f);

        Color iconColor = interactable
            ? Color.white
            : new Color(
                0.8f,
                0.8f,
                0.8f,
                0.6f);

        icon.color  = iconColor;
        label.color = textColor;
        price.color = textColor;
    }
}