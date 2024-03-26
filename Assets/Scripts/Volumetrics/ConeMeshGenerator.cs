using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Light))]
public class ConeMeshGenerator : MonoBehaviour
{
    [SerializeField, Min(3)] private int resolution;
    [SerializeField] private Material material;
    [SerializeField] private LayerMask castLayer;

    [SerializeField] private AngleType angleType;

    [Serializable]
    private enum AngleType
    {
        Spot,
        InnerSpot,
        Average
    }

    private Light coneLight;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private GameObject meshGameObject;
    private int selfInd;

    private void OnEnable()
    {
        meshGameObject?.SetActive(true);
        selfInd = GetComponents<ConeMeshGenerator>().IndexOf(this);

        coneLight = GetComponent<Light>();

        var trm = transform.Find($"{gameObject.name} cone mesh ({selfInd})");
        meshGameObject = trm ? trm.gameObject : new GameObject($"{gameObject.name} cone mesh ({selfInd})");
        meshGameObject.layer = gameObject.layer;

        meshGameObject.transform.SetParent(transform);
        meshGameObject.transform.localRotation = Quaternion.identity;
        meshGameObject.transform.localPosition = Vector3.zero;
        meshGameObject.transform.localScale = Vector3.one;

        if (!meshGameObject.TryGetComponent(out meshRenderer))
            meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
        if (!meshGameObject.TryGetComponent(out meshFilter)) meshFilter = meshGameObject.AddComponent<MeshFilter>();

        if (material) meshRenderer.material = material;
    }

    private void Update()
    {
        if (!meshRenderer || !meshGameObject || !meshFilter) OnEnable();
        if (!coneLight) return;
        if (coneLight.type != LightType.Spot) return;
        if (material)
        {
            material.SetColor("_BaseColor", coneLight.color);
            meshRenderer.material = material;
        }

        var mesh = new Mesh();
        var wPos = transform.position;
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var tris = new List<int>();

        for (var i = 0; i < resolution * 2; i++)
        {
            float angle = coneLight.spotAngle;
            switch (angleType)
            {
                case AngleType.Average:
                    angle = (coneLight.spotAngle + coneLight.innerSpotAngle) / 2f;
                    break;
                case AngleType.InnerSpot:
                    angle = coneLight.innerSpotAngle;
                    break;
                case AngleType.Spot:
                    angle = coneLight.spotAngle;
                    break;
            }

            var dir = Quaternion.Euler(0, 0, 360f / resolution * 2 * i) * new Vector3(
                Mathf.Tan(angle / 2f * Mathf.Deg2Rad) * coneLight.range,
                0,
                coneLight.range
            ).normalized;

            var distance = coneLight.range;
            if (castLayer != 0)
            {
                var rays = Physics.RaycastAll(transform.position, transform.rotation * dir, coneLight.range, castLayer);

                if (rays.Length > 0)
                    distance = rays[0].distance;
            }

            vertices.Add(dir * coneLight.shadowNearPlane);
            uv.Add(new Vector2(0.5f, coneLight.shadowNearPlane / coneLight.range));
            //Debug.Log(uv.Last());
            vertices.Add(dir * distance);
            uv.Add(new Vector2(0.5f, distance / coneLight.range));
        }

        for (var i = 0; i < resolution; i += 2)
        {
            tris.Add(i);
            tris.Add(i + 3);
            tris.Add(i + 1);
            tris.Add(i + 2);
            tris.Add(i + 3);
            tris.Add(i);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();
        mesh.name = "ass";
        meshFilter.mesh = mesh;
        meshFilter.sharedMesh = mesh;
    }

    private void OnDestroy()
    {
        if(meshGameObject)
            DestroyImmediate(meshGameObject);
    }

    private void OnDisable()
    {
        meshGameObject.SetActive(false);
    }
}