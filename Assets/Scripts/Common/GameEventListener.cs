using Objects.Scriptable;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A custom UnityEvent that can pass generic data.
/// </summary>
[System.Serializable]
public class CustomGameEvent : UnityEvent<object> { }

/// <summary>
/// Listens for a GameEvent and invokes a response.
/// </summary>
public class GameEventListener : MonoBehaviour
{
    [Tooltip("The event to listen for.")]
    public GameEvent gameEvent;
    [Tooltip("The response to invoke when the event is raised.")]
    public CustomGameEvent response;

    /// <summary>
    /// Registers the listener with the event.
    /// </summary>
    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    /// <summary>
    /// Unregisters the listener with the event.
    /// </summary>
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    /// <summary>
    /// Called when the event is raised.
    /// </summary>
    /// <param name="data">Data to passed with the event.</param>
    public void OnEventRaised(object data)
    {
        response?.Invoke(data);
    }
}
