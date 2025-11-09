using UnityEngine;

/// <summary>
///     Handles the visual representation and animation of an enemy.
///     Updates animator parameters based on the speed.
/// </summary>
public class EnemyVisual : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");

    [SerializeField]
    private Animator animator;

    [HideInInspector]
    public float speedMultiplier;

    private void FixedUpdate()
    {
        animator.SetFloat(Speed, speedMultiplier);
    }
}