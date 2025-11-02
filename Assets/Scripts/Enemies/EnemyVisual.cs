using UnityEngine;

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