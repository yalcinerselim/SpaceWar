using UnityEngine;

public class DamageController
{
    private readonly int _bulletToUnarmored = 20;
    private readonly int _lethalDamage = 1000;
    
    private int _totalDamage;
    
    public static DamageController Instance { get; } = new DamageController();
    private DamageController()
    {
    }

    public int TakeDamage(GameObject bulletType, GameObject targetType)
    {
        if (bulletType.GetComponent<Bullet>())
        {
            TakeBulletDamage(targetType);
        }
        else if (bulletType.GetComponent<LethalBoundary>() || bulletType.GetComponent<SpaceShipController>())
        {
            TakeLethalDamage();
        }
        else
        {
            TakeZeroDamage();
        }
        
        return _totalDamage;
    }

    private void TakeZeroDamage()
    {
        _totalDamage = 0;
    }
    private void TakeLethalDamage()
    {
        _totalDamage = _lethalDamage;
    }

    private void TakeBulletDamage(GameObject targetType)
    {
        if (targetType.GetComponent<UnarmoredShip>())
        {
            _totalDamage = _bulletToUnarmored;
        }
    }

}
