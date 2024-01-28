using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Light))]
public class ConeMeshGenerator : MonoBehaviour
{
    [SerializeField, Min(3)] private int resolution;
    [SerializeField] private Material material;
    [SerializeField] private LayerMask castLayer;
    private Light coneLight;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private GameObject meshGameObject;

    private void OnEnable()
    {
        coneLight = GetComponent<Light>();

        var trm = transform.Find($"{gameObject.name} cone mesh");
        meshGameObject = trm ? trm.gameObject : new GameObject($"{gameObject.name} cone mesh");
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

        vertices.Add(Vector3.zero);
        uv.Add(new Vector2(0.5f, 0));
        for (var i = 0; i <= resolution; i++)
        {
            var angle = (coneLight.spotAngle + coneLight.innerSpotAngle) / 2f;
            var dir = Quaternion.Euler(0, 0, 360f / resolution * i) * new Vector3(
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

            vertices.Add(dir * distance);
            //uv.Add(new Vector2(0.5f, distance / coneLight.range));
            uv.Add(new Vector2(0.5f, 1));

            tris.Add(0);
            tris.Add(i);
            tris.Add(i + 1);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();
        mesh.name = "ass";
        meshFilter.mesh = mesh;
        meshFilter.sharedMesh = mesh;
    }
}