using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Managers
{
    public class PrefabManager : Singleton<PrefabManager>
    {
        [SerializeField] private PrefabList list;

        private readonly Dictionary<Prefabs, Prefab> _prefabs = new();
        private readonly Dictionary<Prefabs, Queue<GameObject>> _pools = new ();

        /// <summary>
        /// Shortcut method for creating a prefab.
        /// </summary>
        /// <param name="prefab">The type of prefab to create.</param>
        /// <param name="setActive">The active state of the prefab.</param>
        public GameObject Create(Prefabs prefab, bool setActive = true) => Instance.Instantiate(prefab, setActive);

        protected override void OnAwake()
        {
            DontDestroyOnLoad(this);

            foreach (var prefab in list.prefabs)
            {
                _prefabs.Add(prefab.type, prefab);

                // If the prefab should be pooled, create a pool for it.
                if (!prefab.shouldPool) continue;
                var root = GameObject.Find(prefab.objectRoot);

                _pools.Add(prefab.type, new Queue<GameObject>());
                for (var i = 0; i < prefab.initialSize; i++)
                {
                    var newObject = Instantiate(prefab.prefab, root.transform);
                    newObject.SetActive(false);
                    _pools[prefab.type].Enqueue(newObject);
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Creates a new instance of the prefab.
        /// </summary>
        /// <param name="prefab">The prefab type</param>
        /// <param name="setActive">The active state.</param>
        /// <returns>The created object.</returns>
        private GameObject Instantiate(Prefabs prefab, bool setActive = true)
        {
            var prefabData = _prefabs[prefab];

            GameObject newObject;
            if (prefabData.shouldPool)
            {
                var pool = _pools[prefab];

                if (!pool.Peek().activeSelf)
                {
                    // Use the object from the pool.
                    newObject = pool.Dequeue();
                    newObject.SetActive(true);

                    // Reset the transform.
                    newObject.transform.Reset(true, true);
                    // Call reset.
                    var poolObject = newObject.GetComponent<IPoolObject>();
                    poolObject?.Reset();
                }
                else
                {
                    // Create a new object.
                    newObject = Instantiate(prefabData.prefab);
                }

                // Re-add the object to the pool.
                pool.Enqueue(newObject);
            }
            else
            {
                newObject = Instantiate(prefabData.prefab);
            }
            
            newObject.SetActive(setActive);

            if (prefabData.objectRoot == null) return newObject;

            var root = GameObject.Find(prefabData.objectRoot);
            if (root)
            {
                newObject.transform.SetParent(root.transform, false);
            }
            
            return newObject;
        }
    }

    public interface IPoolObject
    {
        /// <summary>
        /// Invoked when the object is reused.
        /// </summary>
        void Reset();
    }
}
