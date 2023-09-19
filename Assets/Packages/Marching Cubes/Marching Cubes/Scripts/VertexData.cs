using UnityEngine;
using Unity.Mathematics;

public struct VertexData
{
	public Vector3 position;
	public Vector3 normal;
	public int2 id;
	public Vector2 uv;
}

public struct TriangleData
{
	public VertexData vertexA;
	public VertexData vertexB;
	public VertexData vertexC;
}