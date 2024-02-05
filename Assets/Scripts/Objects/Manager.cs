using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Defines a basic interface for all managers.
    /// </summary>
    public interface IManager
    {
        public void Initialize();
    }
    
    /// <summary>
    /// Provides a base implementation for all managers.
    /// </summary>
    /// <typeparam name="T">Type of the specific manager class.</typeparam>
    public abstract class Manager<T> : Singleton<T>, IManager where T : MonoBehaviour
    {
        public virtual void Initialize()
        {
            
        }
    }
}