using UnityEngine;

// Script created by https://youtube.com/c/Boxply

public static class CameraPlane
{
    public static Vector3 ViewportToWorldPlanePoint(Camera theCamera, float zDepth, Vector2 viewportCord)
    {
        var angles = ViewportPointToAngle(theCamera, viewportCord);
        var xOffset = Mathf.Tan(angles.x) * zDepth;
        var yOffset = Mathf.Tan(angles.y) * zDepth;
        var cameraPlanePosition = new Vector3(xOffset, yOffset, zDepth);
        cameraPlanePosition = theCamera.transform.TransformPoint(cameraPlanePosition);
        return cameraPlanePosition;
    }

    public static Vector3 ScreenToWorldPlanePoint(Camera camera, float zDepth, Vector3 screenCoord)
    {
        var point = Camera.main.ScreenToViewportPoint(screenCoord);
        return ViewportToWorldPlanePoint(camera, zDepth, point);
    }

    public static Vector2 ViewportPointToAngle(Camera cam, Vector2 viewportCord)
    {
        var adjustedAngle = AngleProportion(cam.fieldOfView / 2, cam.aspect) * 2;
        var xProportion = ((viewportCord.x - .5f) / .5f);
        var yProportion = ((viewportCord.y - .5f) / .5f);
        var xAngle = AngleProportion(adjustedAngle / 2, xProportion) * Mathf.Deg2Rad;
        var yAngle = AngleProportion(cam.fieldOfView / 2, yProportion) * Mathf.Deg2Rad;
        return new Vector2(xAngle, yAngle);
    }

    public static float CameraToPointDepth(Camera cam, Vector3 point)
    {
        var localPosition = cam.transform.InverseTransformPoint(point);
        return localPosition.z;
    }

    public static float AngleProportion(float angle, float proportion)
    {
        var opposite = Mathf.Tan(angle * Mathf.Deg2Rad);
        var oppositeProportion = opposite * proportion;
        return Mathf.Atan(oppositeProportion) * Mathf.Rad2Deg;
    }
}