using UnityEngine;

namespace DefaultNamespace
{
    public class MyGun : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController = null;
        [SerializeField] private GameObject bulletPrefab = null;

        private void OnEnable()
        {
            weaponController.OnShot += OnWeaponShot;
        }

        private void OnDisable()
        {
            weaponController.OnShot -= OnWeaponShot;
        }

        private void OnWeaponShot(Vector3 hitPosition)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.LookAt(hitPosition);
            // TODO: logica de mover bala a posicion
        }

        public void FindAGameObject()
        {
            
        }
    }
}
