using UnityEngine;

public class DragLine
{
    public readonly LineRenderer LineRenderer;
    public readonly Transform LineRenderLocation;

    public DragLine(LineRenderer lineRenderer, Transform lineRenderLocation)
    {
        LineRenderer = lineRenderer;
        LineRenderLocation = lineRenderLocation;
    }
}