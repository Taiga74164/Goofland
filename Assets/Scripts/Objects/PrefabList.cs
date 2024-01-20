using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "Prefabs/List")]
    public class PrefabList : ScriptableObject
    {
        public List<Prefab> prefabs;
    }

    [Serializable]
    public struct Prefab
    {
        [Header("Basic Info")]
        public Prefabs type;
        public GameObject prefab;

        [Header("Pooling")]
        public bool shouldPool;
        public int initialSize;
        public string objectRoot;
    }

    public enum Prefabs
    {
        Player
    }
}
