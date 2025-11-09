using UnityEngine;

/// <summary>
///     Script that shows dissapears after first selection.
///     Provides a tooltip for first selection.
/// </summary>
public class PlacementTutorialUI : MonoBehaviour
{
    public void Start()
    {
        EventBus.Instance.Subscribe<OnFreeBlock2X2Selected>(FreeBlock2X2Selected);
    }

    public void OnDestroy()
    {
        EventBus.Instance.Unsubscribe<OnFreeBlock2X2Selected>(FreeBlock2X2Selected);
    }

    private void FreeBlock2X2Selected()
    {
        Destroy(gameObject);
    }
}