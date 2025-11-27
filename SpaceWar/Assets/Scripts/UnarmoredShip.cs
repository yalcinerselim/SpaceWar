using System;
using UnityEngine;

public class UnarmoredShip : MonoBehaviour
{
    private int _health = 120;

    private void TakeDamage(GameObject otherObject)
    {
        _health -= DamageController.Instance.TakeDamage(otherObject, gameObject);
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
