using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class CustomGameEvent : UnityEvent<Component, object> { }
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent; //the event this object is listening for
    public CustomGameEvent response; //this is the method you want to call

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
