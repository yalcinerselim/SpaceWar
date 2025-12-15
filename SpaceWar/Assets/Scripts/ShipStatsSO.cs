using UnityEngine;

[CreateAssetMenu(fileName = "ShipStatsSO", menuName = "Scriptable Objects/ShipStatsSO")]
public class ShipStatsSO : ScriptableObject
{
    public float BaseSpeed = 3f;
    public float SprintMultiplier = 2f;
    public int Armor = 10;
    public int CollisionEnemyDamage = 1000;
    public float FireRate = 0.5f;
}
