using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
    public class GenerationBounds : MonoBehaviour
    {
        [SerializeField] private bool immerse;
        [SerializeField] private bool showImmerse;
        [SerializeField] private List<Region> regions;
        public bool Immerse => immerse;
        public DotData[] Dots => GenerateDots();

        public struct DotData
        {
            public Vector3 Position;
            public float Diameter;
        }

        private void OnDrawGizmosSelected()
        {
            var pos = transform.position;
            var rot = transform.rotation;

            foreach (var region in regions)
            {
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        region.size.y,
                        region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        -region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        -region.size.y,
                        region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        region.size.y,
                        -region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        region.size.y,
                        -region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        -region.size.y,
                        -region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        -region.size.y,
                        -region.size.z
                    ) + region.offset
                );


                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        region.size.x,
                        -region.size.y,
                        region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        -region.size.x,
                        region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        -region.size.y,
                        region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        -region.size.x,
                        region.size.y,
                        -region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        -region.size.y,
                        -region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        region.size.y,
                        -region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        region.size.x,
                        -region.size.y,
                        -region.size.z
                    ) + region.offset
                );


                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        region.size.x,
                        region.size.y,
                        -region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        -region.size.x,
                        region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        region.size.y,
                        -region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        region.size.x,
                        -region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        region.size.x,
                        -region.size.y,
                        -region.size.z
                    ) + region.offset
                );
                Gizmos.DrawLine(
                    pos + rot * new Vector3(
                        -region.size.x,
                        -region.size.y,
                        region.size.z
                    ) + region.offset,
                    pos + rot * new Vector3(
                        -region.size.x,
                        -region.size.y,
                        -region.size.z
                    ) + region.offset
                );
            }
            return;
            if (!showImmerse) return;

            foreach (var p in Dots)
            {
                Gizmos.DrawSphere(p.Position, p.Diameter / 2f);
            }
        }

        private DotData[] GenerateDots()
        {
            var pos = transform.position;
            var rot = transform.rotation;
            var dots = new List<DotData>();

            foreach (var region in regions)
            {
                for (var x = -region.size.x; x < region.size.x; x += region.diameter)
                {
                    for (var y = -region.size.y; y < region.size.y; y += region.diameter)
                    {
                        for (var z = -region.size.z; z < region.size.z; z += region.diameter)
                        {
                            var dot = new DotData
                            {
                                Position = pos + region.offset + rot * new Vector3(x, y, z) +
                                           (region.diameter / 2f).ToVector3(),
                                Diameter = region.diameter
                            };

                            //if (dots.Where(v => Vector3.Distance(v.Position, dot.Position) <= (region.diameter<v.Diameter?v.Diameter:region.diameter)-(region.diameter<v.Diameter?v.Diameter:region.diameter)*0.01f).ToArray()
                            //    .Length > 0)
                            //    continue;

                            dots.Add(dot);
                        }
                    }
                }
            }

            return dots.ToArray();
        }

        [Serializable]
        private struct Region
        {
            public Vector3 size;
            public Vector3 offset;
            [Min(0.1f)] public float diameter;
        }
    }
}