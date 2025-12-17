using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShipStatsSO", menuName = "Scriptable Objects/EnemyShipStatsSO")]
public class EnemyShipStatsSO : ScriptableObject
{
    public int Health = 100;
    public float BaseSpeed = 3f;
    public int ArmorValue = 100;
    public float FireRate = 0.5f;
    public int CollisionDamage = 30;
}
