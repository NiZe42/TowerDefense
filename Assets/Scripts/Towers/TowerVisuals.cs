using UnityEngine;

/// <summary>
///     Tower visual prefab, that is responsible for controlling from what point does the shooting start.
/// </summary>
public class TowerVisuals : MonoBehaviour
{
    public Transform firePoint;

    // if tower is aimed visually at the target, we can proceed with shooting.
    public bool isAimed { get; private set; }
}