using UnityEngine;

[CreateAssetMenu(fileName = "ArmoryConfig", menuName = "Bullet Blow / ArmoryConfig", order = 0)]
public class ArmoryConfig : ScriptableObject
{
    public WeaponConfig[] weapons;
}
