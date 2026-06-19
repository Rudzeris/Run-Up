using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct TypePrefab
    {
        public EntityType type;
        public GameObject prefab;
    }
}