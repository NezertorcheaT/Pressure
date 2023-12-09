using System;
using UnityEngine;

namespace Generation
{
    [Serializable]
    public struct GenerationBuildable
    {
        public bool process;
        [Min(0)] public int count;
        public GenerationBounds prefab;
        [Min(0)] public float normalOffset;
        public bool normalRotate;
        public ScriptableObject placer;
    }
}