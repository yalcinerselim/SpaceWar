using UnityEngine;

public class UnarmoredShip : MonoBehaviour
{
    private int _health = 120;
    
    private DamageController _damageController = DamageController.Instance;

    private void TakeDamage(GameObject otherObject)
    {
        _health -= _damageController.TakeDamage(otherObject, gameObject);
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage(other.gameObject);
    }
}
