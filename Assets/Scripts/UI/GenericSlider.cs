using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Generic Slider used by slider prefab.
/// </summary>
[RequireComponent(typeof(Slider))]
public class GenericSlider : MonoBehaviour
{
    [SerializeField]
    private float smoothSpeed;

    private Slider slider;
    private float targetValue;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    private void FixedUpdate()
    {
        if (Math.Abs(slider.value - targetValue) > .01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetValue, smoothSpeed * Time.deltaTime);
        }
    }

    public void SetTargetValue(float normalizedHealth)
    {
        targetValue = normalizedHealth;
    }
}