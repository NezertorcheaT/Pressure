using UnityEngine;

public class DragLine
{
    public LineRenderer lr;
    public Transform lineRenderLocation;

    public DragLine(LineRenderer lr, Transform lineRenderLocation)
    {
        this.lr = lr;
        this.lineRenderLocation = lineRenderLocation;
    }
}