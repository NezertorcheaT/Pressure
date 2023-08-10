using System.Collections.Generic;
using UnityEngine;

public class GenerationBounds : MonoBehaviour
{
    [SerializeField] private bool immerse;
    [SerializeField] private bool showImmerse;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 offset;
    [SerializeField, Min(0.01f)] private float distribution = 1;
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

        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                size.y,
                size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                -size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                -size.y,
                size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                size.y,
                -size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                size.y,
                -size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                -size.y,
                -size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                -size.y,
                -size.z
            ) + offset
        );


        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                size.x,
                -size.y,
                size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                -size.x,
                size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                -size.y,
                size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                -size.x,
                size.y,
                -size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                -size.y,
                -size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                size.y,
                -size.z
            ) + offset,
            pos + rot * new Vector3(
                size.x,
                -size.y,
                -size.z
            ) + offset
        );


        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                size.x,
                size.y,
                -size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                -size.x,
                size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                size.y,
                -size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                size.x,
                -size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                size.x,
                -size.y,
                -size.z
            ) + offset
        );
        Gizmos.DrawLine(
            pos + rot * new Vector3(
                -size.x,
                -size.y,
                size.z
            ) + offset,
            pos + rot * new Vector3(
                -size.x,
                -size.y,
                -size.z
            ) + offset
        );

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

        for (var x = -size.x; x < size.x; x += distribution)
        {
            for (var y = -size.y; y < size.y; y += distribution)
            {
                for (var z = -size.z; z < size.z; z += distribution)
                {
                    dots.Add(new DotData()
                    {
                        Position = pos + offset + rot * new Vector3(x, y, z) + (distribution/2f).ToVector3(),
                        Diameter = distribution
                    });
                }
            }
        }

        return dots.ToArray();
    }
}