using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    
    [SerializeField] private DestructibleObject destructibleObject;

    private void Awake()
    {
        if (destructibleObject == null)
        {
            destructibleObject = GetComponentInParent<DestructibleObject>();
        }
    }

    private void OnEnable()
    {
        if (destructibleObject is not null)
            destructibleObject.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (destructibleObject is not null)
            destructibleObject.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(float healthPercent)
    {
        healthBarSlider.value = healthPercent;
    }
}
