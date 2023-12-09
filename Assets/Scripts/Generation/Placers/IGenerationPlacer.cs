using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Generation.Placers
{
    /// <summary>
    /// Must be on Scriptable Object
    /// </summary>
    internal interface IGenerationPlacer
    {
        /// <summary>
        /// Must be async
        /// </summary>
        /// <param name="buildable"></param>
        /// <param name="ImmerseTerrain">Terrain immerse action</param>
        /// <param name="InstantiatePrefab">Instantiating function</param>
        /// <param name="boundsSize">Terrain bounds</param>
        /// <returns></returns>
        Task PlaceRandom(GenerationBuildable buildable, Action<GenerationBounds> ImmerseTerrain,
            Func<UnityEngine.Object, Vector3, Quaternion, Transform,GameObject> InstantiatePrefab,float boundsSize);
    }
}