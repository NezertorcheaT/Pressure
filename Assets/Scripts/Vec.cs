using UnityEngine;

public static class Vec
{
    public static Vector3 xyz(this Vector3 t) => new Vector3(t.x, t.y, t.z);
    public static Vector3 xzy(this Vector3 t) => new Vector3(t.x, t.z, t.y);
    public static Vector3 yxz(this Vector3 t) => new Vector3(t.y, t.x, t.z);
    public static Vector3 zxy(this Vector3 t) => new Vector3(t.z, t.x, t.y);
    public static Vector3 xxx(this Vector3 t) => new Vector3(t.x, t.x, t.x);
    public static Vector3 yyy(this Vector3 t) => new Vector3(t.y, t.y, t.y);
    public static Vector3 zzz(this Vector3 t) => new Vector3(t.z, t.z, t.z);
    public static Vector3 xxy(this Vector3 t) => new Vector3(t.x, t.x, t.y);
    public static Vector3 xxz(this Vector3 t) => new Vector3(t.x, t.x, t.z);
    public static Vector3 yyx(this Vector3 t) => new Vector3(t.y, t.y, t.x);
    public static Vector3 yyz(this Vector3 t) => new Vector3(t.y, t.y, t.z);
    public static Vector3 zzx(this Vector3 t) => new Vector3(t.z, t.z, t.x);
    public static Vector3 zzy(this Vector3 t) => new Vector3(t.z, t.z, t.y);

    public static Vector2 xy(this Vector3 t) => new Vector2(t.x, t.y);
    public static Vector2 xz(this Vector3 t) => new Vector2(t.x, t.z);
    public static Vector2 yx(this Vector3 t) => new Vector2(t.y, t.x);
    public static Vector2 zx(this Vector3 t) => new Vector2(t.z, t.x);
    public static Vector2 xx(this Vector3 t) => new Vector2(t.x, t.x);
    public static Vector2 yy(this Vector3 t) => new Vector2(t.y, t.y);
    public static Vector2 zz(this Vector3 t) => new Vector2(t.z, t.z);
    public static Vector2 zy(this Vector3 t) => new Vector2(t.z, t.y);
    public static Vector2 yz(this Vector3 t) => new Vector2(t.y, t.z);

    public static Vector2 xx(this Vector2 t) => new Vector2(t.x, t.x);
    public static Vector2 yy(this Vector2 t) => new Vector2(t.y, t.y);
    public static Vector2 yx(this Vector2 t) => new Vector2(t.y, t.x);
    public static Vector2 xy(this Vector2 t) => new Vector2(t.x, t.y);

    public static Vector3 xyy(this Vector2 t) => new Vector3(t.x, t.y, t.y);
    public static Vector3 yxx(this Vector2 t) => new Vector3(t.y, t.x, t.x);
    public static Vector3 xxx(this Vector2 t) => new Vector3(t.x, t.x, t.x);
    public static Vector3 yyy(this Vector2 t) => new Vector3(t.y, t.y, t.y);

    public static float x(this float t) => t;
    public static Vector2 xx(this float t) => new Vector2(t, t);
    public static Vector3 xxx(this float t) => new Vector3(t, t, t);
    public static int x(this int t) => t;
    public static Vector2 xx(this int t) => new Vector2(t, t);
    public static Vector3 xxx(this int t) => new Vector3(t, t, t);
    public static float x(this bool t) => t ? 1f : 0f;
    public static Vector2 xx(this bool t) => new Vector2(t ? 1 : 0, t ? 1 : 0);
    public static Vector3 xxx(this bool t) => new Vector3(t ? 1 : 0, t ? 1 : 0, t ? 1 : 0);
}