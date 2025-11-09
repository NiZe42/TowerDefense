using TMPro;
using UnityEngine;

/// <summary>
///     Text that is used for dissapearing money pops up when enemy dies.
///     Goes up for some time and then destroyes itself.
/// </summary>
public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private float floatSpeed;

    [SerializeField]
    private float duration;

    [SerializeField]
    private TextMeshProUGUI text;

    private void Update()
    {
        transform.position += floatSpeed * Time.deltaTime * Vector3.up;

        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(string message, Color color)
    {
        if (text == null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        text.text  = message;
        text.color = color;
    }
}