using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;
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
    [SerializeField] private float boatSizeMultiplyer = 29.14999f;
    [SerializeField] private Color color = Color.yellow;
    [SerializeField, Min(2)] private int textureSize = 128;
    [SerializeField] private float speed = 0.1f;
    [SerializeField, Min(0)] private float cleanDelay = 3f;
    [SerializeField, Min(0.000001f)] private float cleanTickTime = 0.1f;
    [SerializeField] private Transform origin;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider[] colliders;

    private Bounds _bounds;
    private float _angle;
    private bool _enable;
    private Texture2D _texture;

    private void Start()
    {
        _texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        _texture.filterMode = FilterMode.Point;
        _texture.Apply(false);
        distance = maxDistance;

        Fill();
        CalculateBounds();
    }

    public void Toggle() => _enable = !_enable;

    public void Off() => _enable = false;
    public void On() => _enable = true;

    private void CalculateBounds()
    {
        _bounds = new Bounds();
        foreach (var collider in colliders)
        {
            _bounds.Encapsulate(collider.bounds);
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
        if(!_enable) return;
        _angle = Mathf.Repeat(_angle + speed, 360);
        arrow.localRotation = Quaternion.Euler(0, 0, -_angle + 90);
        boat.localRotation = Quaternion.Euler(0, 0, origin.rotation.eulerAngles.y);
        boat.localScale =
            new Vector3(_bounds.size.x * 2f / boatSizeMultiplyer, _bounds.size.x * 2f / boatSizeMultiplyer) / distance;

        boat.localScale = new Vector3(boat.localScale.x, boat.localScale.y, 1);

        for (float i = -height / 2; i < height / 2; i += _bounds.size.y / (float) textureSize * heightOffsetMultiplier)
        {
            DrawRayPoint(i);
        }

        image.texture = _texture;
    }

    private void Fill()
    {
        var fillColorArray = _texture.GetPixels();
        for (var i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = Color.black;
        }

        _texture.SetPixels(fillColorArray);
        _texture.Apply(false);
    }

    private async void Draw(int x, int y)
    {
        if (!_texture) return;
        _texture.SetPixel(x, y, color);
        _texture.Apply(false);

        for (var sec = 0f; sec <= cleanDelay; sec += cleanTickTime)
        {
            await Task.Delay((int) (1000f * cleanTickTime));
            if (!_texture) return;
            _texture.SetPixel(x, y, color * (1f - sec / cleanDelay));
            _texture.Apply(false);
        }

        if (!_texture) return;
        _texture.SetPixel(x, y, Color.black);
        _texture.Apply(false);
    }

    private void DrawRayPoint(float yOffset)
    {
        var rotate = Angle2Vec3(_angle);

        Debug.DrawRay(origin.position + new Vector3(0, yOffset, 0), rotate * distance, Color.yellow);

        var hit = Physics.RaycastAll(origin.position + new Vector3(0, yOffset, 0), rotate, distance, layer);

        if (hit.Length == 0) return;

        var position = (origin.position - hit[0].point) / distance;

        position *= ((float) textureSize) / 2f;
        position += new Vector3(((float) textureSize) / 2f, 0, ((float) textureSize) / 2f);
        Draw((int) position.x, (int) position.z);
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