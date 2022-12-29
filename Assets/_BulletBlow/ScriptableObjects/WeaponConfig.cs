using UnityEngine;

public enum WeaponType
{
    Melee, Pistol, Rifle
}

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Bullet Blow / WeaponConfig", order = 1)]
public class WeaponConfig : ScriptableObject
{
    public string weaponName = "";
    [Header("Collisions Config")]
    public WeaponType type;
    // public LayerMask collisionLayers;
    [Space(20)]
    [Header("Balancing")]
    [Range(1, 100)]
    public int ammoMaximum;
    [Range(1, 20)]
    public int ammoMagazine;
    [Range(.1f, 1f)]
    public float speedMultiplier;
    [Range(1f, 100f)]
    public float range;
    [Range(1f, 100f)]
    public float strength;
    [Range(.1f, 1f)]
    public float accuracy;
    [Space(30)]
    [Header("Assets")]
    public GameObject prefab;
    public AudioClip sfxShot;
    public AudioClip sfxEmpty;
    public AudioClip sfxReload;
}
