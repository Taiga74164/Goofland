using UnityEngine.InputSystem;

namespace Managers
{
    /// <summary>
    /// Manages inputs globally for the game.
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        private InputActions _actions;

        // Player actions.
        public static InputAction Move => Instance._actions.Player.Movement;
        public static InputAction Jump => Instance._actions.Player.Jump;
        public static InputAction Crouch => Instance._actions.Player.Crouch;
        public static InputAction Run => Instance._actions.Player.Run;
        public static InputAction Attack => Instance._actions.Player.Attack;
        // Weapon selection.
        public static InputAction SelectPie => Instance._actions.Player.SelectPie;
        public static InputAction SelectWaterGun => Instance._actions.Player.SelectWaterGun;
        public static InputAction SelectBananaPeel => Instance._actions.Player.SelectBananaPeel;
        
        // User Interface actions.
        public static InputAction Return => Instance._actions.Interface.Return;

        protected override void OnAwake() => _actions = new InputActions();

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();
    }
}