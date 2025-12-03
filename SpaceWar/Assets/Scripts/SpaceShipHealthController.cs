using System;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceShipHealthController : MonoBehaviour
{
    private float _maxHealth = 100f;
    private float _currentHealth;
    private float _minHealth = 0f;

    private DamageController _damageController = DamageController.Instance;
    
    
    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamageHandler(GameObject bulletObject)
    {
        
        _currentHealth -= _damageController.TakeDamage(bulletObject, transform.parent.gameObject);
        if (_currentHealth <= _minHealth)
        {
            Debug.Log("Ship Destroyed");
        }
    }
    
}
