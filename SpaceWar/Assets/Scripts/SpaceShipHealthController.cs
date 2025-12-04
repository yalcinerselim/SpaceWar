using UnityEngine;

public class SpaceShipHealthController : MonoBehaviour
{
    private float _maxHealth = 100f;
    private float _currentHealth;
    private float _minHealth = 0f;
    
    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamageHandler(int damageAmount)
    {
        
        _currentHealth -= damageAmount;
        if (_currentHealth <= _minHealth)
        {
            Debug.Log("Ship Destroyed");
        }
    }
    
}
