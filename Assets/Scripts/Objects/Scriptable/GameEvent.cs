using System.Collections.Generic;
using UnityEngine;

namespace Objects.Scriptable
{
    /// <summary>
    /// Represents a game event for use in the game.
    /// </summary>
    [CreateAssetMenu(menuName = "GameEvent")] 
    public class GameEvent : ScriptableObject
    {
        public List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise(object data)
        {
            foreach (var listener in listeners)
            {
                listener.OnEventRaised(data);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}
