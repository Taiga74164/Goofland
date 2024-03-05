using System;
using UnityEngine;
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
        public static InputAction AimUp => Instance._actions.Player.AimUp;
        public static InputAction AimDown => Instance._actions.Player.AimDown;
        public static InputAction AngleUp => Instance._actions.Player.AngleUp;
        public static InputAction AngleDown => Instance._actions.Player.AngleDown;

        // User Interface actions.
        public static InputAction Return => Instance._actions.UI.Return;
        public static InputAction Cancel => Instance._actions.UI.Cancel;

        protected override void OnAwake() => _actions = new InputActions();

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();
        
        /// <summary>
        /// Special singleton initializer method.
        /// </summary>
        public new static void Initialize()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Managers/InputManager");
            if (prefab == null) throw new Exception("Missing InputManager prefab!");

            var instance = Instantiate(prefab);
            if (instance == null) throw new Exception("Failed to instantiate InputManager prefab!");

            instance.name = "Managers.InputManager (Singleton)";
        }
    }
}