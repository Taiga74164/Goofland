using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    /// <summary>
    /// Manages inputs globally for the game.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        private static InputActions _actions;

        // Player actions.
        public static InputAction Move;
        public static InputAction Jump;
        public static InputAction Crouch;
        public static InputAction Run;
        public static InputAction Attack;
        // Weapon selection.
        public static InputAction SelectPie;
        public static InputAction SelectWaterGun;
        public static InputAction SelectBananaPeel;
        
        // User Interface actions.
        public static InputAction Return;

        private void Awake()
        {
            // Create the input actions asset.
            _actions = new InputActions();

            // Update the player actions.
            Move = _actions.Player.Movement;
            Jump = _actions.Player.Jump;
            Crouch = _actions.Player.Crouch;
            Run = _actions.Player.Run;
            Attack = _actions.Player.Attack;
            SelectPie = _actions.Player.SelectPie;
            SelectWaterGun = _actions.Player.SelectWaterGun;
            SelectBananaPeel = _actions.Player.SelectBananaPeel;
            
            // Update the user interface actions.
            Return = _actions.Interface.Return;
        }

        #region Boilerplate

        private void OnEnable()
        {
            // Enable the input actions.
            _actions.Enable();
        }

        private void OnDisable()
        {
            // Disable the input actions.
            _actions.Disable();
        }

        #endregion
    }
}