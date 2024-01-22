using Managers;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{ 
    public class WeaponController : MonoBehaviour
    {
        public WeaponType currentWeapon;
        private GameObject[] _weapons;
        
        #region Input Actions
    
        private InputAction _selectPie, _selectWaterGun, _selectBananaPeel;
    
        #endregion

        private void Start()
        {
            // Create the weapons.
            _weapons = new[]
            {
                PrefabManager.Instance.Create(Prefabs.Pie, false),
                PrefabManager.Instance.Create(Prefabs.WaterGun, false),
                PrefabManager.Instance.Create(Prefabs.BananaPeel, false),
            };
            
            // Set up input action references.
            _selectPie = InputManager.SelectPie;
            _selectWaterGun = InputManager.SelectWaterGun;
            _selectBananaPeel = InputManager.SelectBananaPeel;
            
            // Listen for input actions to select weapons.
            _selectPie.performed += _ => SelectWeapon(WeaponType.Pie);
            _selectWaterGun.performed += _ => SelectWeapon(WeaponType.WaterGun);
            _selectBananaPeel.performed += _ => SelectWeapon(WeaponType.BananaPeel);
        }

        private void Update()
        {
            Debug.Log($"currentWeapon: {currentWeapon}");
        }

        private void SelectWeapon(WeaponType weaponType)
        {
            currentWeapon = weaponType;
            SwitchWeapon();
        }
        
        private void SwitchWeapon()
        {            
            switch (currentWeapon)
            {
                case WeaponType.Pie:
                    ActivateWeapon(WeaponType.Pie);
                    break;
                case WeaponType.WaterGun:
                    ActivateWeapon(WeaponType.WaterGun);
                    break;
                case WeaponType.BananaPeel:
                    ActivateWeapon(WeaponType.BananaPeel);
                    break;
            }
        }

        private void ActivateWeapon(WeaponType weaponType)
        {
            foreach (var weapon in _weapons)
                weapon.SetActive(weapon.name.Contains(weaponType.ToString()));
        }
    }
    
    public enum WeaponType
    {
        Pie,
        WaterGun,
        BananaPeel,
    }
}