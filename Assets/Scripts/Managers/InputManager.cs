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
        //public static InputAction Aim => Instance._actions.Player.Aim;
        public static InputAction AimUp => Instance._actions.Player.AimUp;
        public static InputAction AimDown => Instance._actions.Player.AimDown;
        public static InputAction AngleUp => Instance._actions.Player.AngleUp;
        public static InputAction AngleDown => Instance._actions.Player.AngleDown;

        // User Interface actions.
        public static InputAction Return => Instance._actions.UI.Return;

        protected override void OnAwake() => _actions = new InputActions();

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();
    }
}