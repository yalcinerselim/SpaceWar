using UnityEngine;

public class DamageController
{
    private readonly int _bulletToUnarmored = 20;
    private readonly int _lethalDamage = 1000;
    private readonly int _collisionDamage = 33;
    private readonly int _bulletToSpaceShip = 5;
    
    private int _totalDamage;
    
    public static DamageController Instance { get; } = new DamageController();
    private DamageController()
    {
    }

    public int TakeDamage(GameObject bulletType, GameObject targetType)
    {
        if (targetType.GetComponent<SpaceShipController>())
        {
            SpaceShipControllerDamageHandler(bulletType);
        }
        else if (targetType.GetComponent<UnarmoredShip>())
        {
            UnarmoredShipDamageHandler(bulletType);
        }
        
        return _totalDamage;
    }

    private void SpaceShipControllerDamageHandler(GameObject bullet)
    {
        if (bullet.GetComponent<Bullet>())
        {
            _totalDamage = _bulletToSpaceShip;
            
        }
        else if (bullet.GetComponent<UnarmoredShip>())
        {
            _totalDamage = _collisionDamage;
        }
    }

    private void UnarmoredShipDamageHandler(GameObject bullet)
    {
        if (bullet.GetComponent<Bullet>())
        {
            _totalDamage = _bulletToUnarmored;
        }
        else if (bullet.GetComponent<LethalBoundary>()|| bullet.GetComponent<SpaceShipController>())
        {
            _totalDamage = _lethalDamage;
        }
        else
        {
            TakeZeroDamage();
        }
    }

    private void TakeZeroDamage()
    {
        _totalDamage = 0;
    }

}
