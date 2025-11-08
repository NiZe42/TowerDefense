using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField]
    private List<Component> serializedTargets;

    private readonly Dictionary<Type, List<IEffect>> activeEffects =
        new Dictionary<Type, List<IEffect>>();

    private readonly Dictionary<Type, Component> targetsByType = new Dictionary<Type, Component>();

    private void Awake()
    {
        foreach (Component component in serializedTargets)
        {
            if (component is null)
            {
                continue;
            }

            Type concreteType = component.GetType();

            TryRegisterTarget(concreteType, component);

            Type baseType = concreteType.BaseType;
            while (baseType != null && baseType != typeof(MonoBehaviour) &&
                baseType != typeof(Component))
            {
                TryRegisterTarget(baseType, component);
                baseType = baseType.BaseType;
            }
        }
    }

    private void Update()
    {
        foreach (List<IEffect> effects in activeEffects.Select(kvp => kvp.Value))
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                effects[i].Tick();
            }
        }
    }

    private void TryRegisterTarget(Type type, Component component)
    {
        if (targetsByType.TryAdd(type, component))
        {
            activeEffects[type] = new List<IEffect>();
        }
    }

    public void AddEffect<T>(Effect<T> effect) where T : MonoBehaviour
    {
        Type type = typeof(T);

        if (!targetsByType.ContainsKey(type))
        {
            Debug.LogWarning($"No component of type {type} assigned in EffectController.");
            return;
        }

        List<IEffect> effectsForTarget = activeEffects[type];

        // Need this to enforce only 1 effect
        Type    effectType     = effect.GetType();
        IEffect existingEffect = effectsForTarget.FirstOrDefault(e => e.GetType() == effectType);

        if (existingEffect != null)
        {
            existingEffect.StopEffect();
            effectsForTarget.Remove(existingEffect);
        }

        effectsForTarget.Add(effect);
        effect.OnCompleted += OnEffectEnded;
    }

    private void OnEffectEnded(IEffect effect)
    {
        effect.OnCompleted -= OnEffectEnded;

        Type targetType = effect.TargetType;
        activeEffects[targetType].Remove(effect);
    }

    public void ClearEffects() { }
}