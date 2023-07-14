using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform boat;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float maxDistance;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float heightOffsetMultiplier = 5f;
    [SerializeField] private Color color = Color.yellow;
    [SerializeField, Min(2)] private int textureSize = 128;
    [SerializeField] private float speed = 0.1f;
    [SerializeField, Min(0)] private float cleanDelay = 3f;
    [SerializeField, Min(0.000001f)] private float cleanTickTime = 0.1f;
    [SerializeField] private Transform origin;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider[] colliders;
    private Bounds bounds;
    private float angle;
    private Texture2D texture;

    private void Start()
    {
        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        texture.Apply(false);
        distance = maxDistance;

        Fill();
        CalculateBounds();
    }
    private void CalculateBounds()
    {
        bounds = new Bounds();
        foreach (var collider in colliders)
        {
            bounds.Encapsulate(collider.bounds);
        }
    }

    public void AddDistance(float speed) => distance = Mathf.Clamp(distance + speed, 15, maxDistance);
    public void ReduceDistance(float speed) => AddDistance(-speed);

    private Vector3 Angle2Vec3(float a)
    {
        return new Vector3(Mathf.Cos(a * Mathf.Deg2Rad), 0, Mathf.Sin(a * Mathf.Deg2Rad)).normalized;
    }
    private void FixedUpdate()
    {
        angle = Mathf.Repeat(angle + speed, 360);
        arrow.localRotation = Quaternion.Euler(0, 0, -angle + 90);
        boat.localRotation = Quaternion.Euler(0, 0, origin.rotation.eulerAngles.y);
        boat.localScale = new Vector3(bounds.size.x * 2f / 16.862f, bounds.size.z * 2f / 29.14999f) / distance;

        boat.localScale = new Vector3(boat.localScale.x, boat.localScale.y, 1);

        for (float i = -height / 2; i < height / 2; i += bounds.size.y / (float)textureSize * heightOffsetMultiplier)
        {
            DrawRayPoint(i);
        }

        image.texture = texture;
    }
    private void Fill()
    {
        var fillColorArray = texture.GetPixels();
        for (var i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = Color.black;
        }
        texture.SetPixels(fillColorArray);
        texture.Apply(false);
    }
    private IEnumerator Draw(int x, int y)
    {
        texture.SetPixel(x, y, color);
        texture.Apply(false);

        for (float sec = 0; sec <= cleanDelay; sec += 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            texture.SetPixel(x, y, color * (1 - sec / cleanDelay));
            texture.Apply(false);
        }

        texture.SetPixel(x, y, Color.black);
        texture.Apply(false);
    }
    private void DrawRayPoint(float yOffset)
    {
        var rotate = Angle2Vec3(angle);
        //var rotate = Angle2Vec3(angle - origin.rotation.eulerAngles.y);
        Debug.DrawRay(origin.position + new Vector3(0, yOffset, 0), rotate * distance, Color.yellow);

        var hit = Physics.RaycastAll(origin.position + new Vector3(0, yOffset, 0), rotate, distance, layer);
        if (hit.Length != 0)
        {
            var position = ((origin.position - hit[0].point) / distance);
            //position = Quaternion.Euler(0, origin.rotation.eulerAngles.y, 0) * position;
            position *= ((float)textureSize / 2);
            position += new Vector3(((float)textureSize / 2), 0, ((float)textureSize / 2));
            StartCoroutine(Draw((int)position.x, (int)position.z));
        }
    }
}
static class Float2Vec
{
    public static Vector3 ToVector3(this float f) => new Vector3(f, f, f);
    public static Vector2 ToVector2(this float f) => new Vector2(f, f);

    public static Vector3 ToVector3(this int f) => new Vector3(f, f, f);
    public static Vector2 ToVector2(this int f) => new Vector2(f, f);

    public static Vector3Int ToVector3Int(this int f) => new Vector3Int(f, f, f);
    public static Vector2Int ToVector2Int(this int f) => new Vector2Int(f, f);
}