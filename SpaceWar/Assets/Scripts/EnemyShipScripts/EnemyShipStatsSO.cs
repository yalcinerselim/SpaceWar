using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShipStatsSO", menuName = "Scriptable Objects/EnemyShipStatsSO")]
public class EnemyShipStatsSO : ScriptableObject
{
    public int Health = 100;
    public float BaseSpeed = 3f;
    public float FastSpeed = 4.5f;
    public float LightArmor = 5;
    public float HeavyArmor = 15;
    public float FireRate = 0.5f;
    public int CollisionDamage = 30;
}
