using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public event Action<Vector3> OnShot;
    public event Action<int, int> OnBulletsUpdated;
    
    [SerializeField] private WeaponConfig weaponDefault = null;
    [SerializeField] private WeaponConfig weapon = null;
    [Space(20)]
    [SerializeField] private Transform mountPoint = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private Camera fpsCam = null;
    [SerializeField] private LayerMask validShotLayerMask;

    private int bullets;
    private int magazine;

    #region Unity Methods

    private void Awake()
    {
        if (weaponDefault == null)
        {
            Debug.LogError($"{GetType().Name} - weaponDefault cannot be null");
        }
    }

    private void Start()
    {
        Release();
    }

    #endregion

    #region Public Methods

    public void Set(WeaponConfig config)
    {
        weapon = config;
        SetUpWeapon(weapon);
    }

    public void Release()
    {
        weapon = weaponDefault;
        SetUpWeapon(weapon);
    }

    public void Shoot()
    {
        var audioClip = weapon.sfxEmpty;
        if (magazine > 0)
        {
            magazine = Mathf.Clamp(--magazine, 0, weapon.ammoMagazine);
            audioClip = weapon.sfxShot;
        }
        
        audioSource.PlayOneShot(audioClip);
        
        OnBulletsUpdated?.Invoke(magazine, bullets);
    }

    public void Reload()
    {
        var diff = weapon.ammoMagazine - magazine;
        magazine += diff;
        bullets -= diff;
        audioSource.PlayOneShot(weapon.sfxReload);
        OnBulletsUpdated?.Invoke(magazine, bullets);
    }

    #endregion

    #region Private Methods

    private void SetUpWeapon(WeaponConfig config)
    {
        var oldWeapon = mountPoint.GetChild(0).gameObject;
        Destroy(oldWeapon);

        
        var newWeapon = Instantiate(config.prefab, mountPoint);
        bullets = weapon.ammoMaximum;
        Reload();
    }

    #endregion


}
