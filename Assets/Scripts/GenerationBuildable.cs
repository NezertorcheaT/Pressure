using System;
using UnityEngine;

[Serializable]
internal struct GenerationBuildable
{
    [Min(0)] public int count;
    public GenerationBounds prefab;
    [Min(0)] public float normalOffset;
    public bool normalRotate;
}