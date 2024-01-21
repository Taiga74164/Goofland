using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages inputs globally for the game.
/// </summary>
public class InputManager : MonoBehaviour
{
    private static InputActions _actions;

    // Player actions.
    public static InputAction move;
    public static InputAction jump;
    public static InputAction run;

    private void Awake()
    {
        // Create the input actions asset.
        _actions = new InputActions();

        // Update the player actions.
        move = _actions.Player.Movement;
        jump = _actions.Player.Jump;
        run = _actions.Player.Run;
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