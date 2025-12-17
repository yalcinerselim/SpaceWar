using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class DestructibleObject : MonoBehaviour, IDamageable
{
    public event Action<float> OnHealthChanged;
    
    protected int currentHealth;
    protected int maxHealth;
    protected int armorValue;

    protected void InitializeHealth(int healthValue, int armorValue)
    {
        maxHealth = healthValue;
        currentHealth = maxHealth;
        this.armorValue = armorValue;
        OnHealthChanged?.Invoke(1f);
    }
    
    
    public virtual void TakeDamage(int damageAmount)
    {
        damageAmount -= armorValue;
        currentHealth -= damageAmount;
        Debug.Log(currentHealth);
        
        float healthPercent = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(healthPercent);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
