using Managers;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{ 
    public class WeaponController : MonoBehaviour
    {
        public WeaponType currentWeapon;

        #region Input Actions
    
        private InputAction _selectPie, _selectWaterGun, _selectBananaPeel;
    
        #endregion

        #region Pie Properties

        private float _pieCooldownDuration = 1.0f;
        private float _lastPieThrownTime = -1.0f;

        #endregion
        
        #region Water Gun Properties

        public WaterGun waterGun;

        #endregion

        #region Banana Peel Properties

        private bool _isLandminePlaced;

        #endregion
        
        private void Start()
        {
            // Set up input action references.
            _selectPie = InputManager.SelectPie;
            _selectWaterGun = InputManager.SelectWaterGun;
            _selectBananaPeel = InputManager.SelectBananaPeel;
            
            // Listen for input actions to select weapons.
            _selectPie.performed += _ => SelectWeapon(WeaponType.Pie);
            _selectWaterGun.performed += _ => SelectWeapon(WeaponType.WaterGun);
            _selectBananaPeel.performed += _ => SelectWeapon(WeaponType.BananaPeel);
            
            // Create water gun for later use.
            waterGun = PrefabManager.Instance.Create(Prefabs.WaterGun, false).GetComponent<WaterGun>();
        }

        private void Update()
        {
            waterGun.gameObject.SetActive(currentWeapon == WeaponType.WaterGun);
            Debug.Log($"currentWeapon: {currentWeapon}");
        }

        /// <summary>
        /// Selects the weapon once the player presses the corresponding button.
        /// </summary>
        /// <param name="weaponType">The weapon type.</param>
        private void SelectWeapon(WeaponType weaponType)
        {
            currentWeapon = weaponType;
            SwitchWeapon();
        }
        
        /// <summary>
        /// Responsible for giving feedback to the player on the HUD.
        /// </summary>
        private void SwitchWeapon()
        {            
            switch (currentWeapon)
            {
                case WeaponType.Pie:
                    // HUD
                    break;
                case WeaponType.WaterGun:
                    // HUD
                    break;
                case WeaponType.BananaPeel:
                    // HUD
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Spawns the weapon. This is called from the PlayerController. Mainly used for Pie and BananaPeel.
        /// </summary>
        /// <param name="weaponType">The weapon type.</param>
        public void SpawnWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Pie:
                    HandlePieThrow();
                    break;
                case WeaponType.BananaPeel:
                    HandleLandmine();
                    _isLandminePlaced = !_isLandminePlaced;
                    break;
            }
        }
        
        private void HandlePieThrow()
        {
            if (!IsPieReady()) return;
            
            PrefabManager.Instance.Create(Prefabs.Pie);
            _lastPieThrownTime = Time.time;
        }
        
        private bool IsPieReady() => Time.time - _lastPieThrownTime >= _pieCooldownDuration;

        private void HandleLandmine()
        {
            if (!_isLandminePlaced)
            {
                var landmine = PrefabManager.Instance.Create(Prefabs.BananaPeel);
                landmine.transform.position = transform.position;
            }
            else
            {
                var landmine = FindObjectOfType<BananaPeel>();
                if (landmine != null)
                    landmine.Explode();
                else
                    Debug.Log("No landmine found in the scene.");
            }
        }
    }
    
    public enum WeaponType
    {
        None,
        Pie,
        WaterGun,
        BananaPeel,
    }
}