using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    private const int ThreadGroupSize = 1024;

    public BoidSettings settings;
    public ComputeShader compute;
    public Boid[] boids;

    private void Update()
    {
        if (boids == null) return;
        if (boids.Length == 0) return;

        var numBoids = boids.Length;
        var boidData = new BoidData[numBoids];

        for (var i = 0; i < boids.Length; i++)
        {
            boidData[i].position = boids[i].position;
            boidData[i].direction = boids[i].forward;
        }

        var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
        boidBuffer.SetData(boidData);

        compute.SetBuffer(0, "boids", boidBuffer);
        compute.SetInt("numBoids", boids.Length);
        compute.SetFloat("viewRadius", settings.perceptionRadius);
        compute.SetFloat("avoidRadius", settings.avoidanceRadius);

        var threadGroups = Mathf.CeilToInt(numBoids / (float) ThreadGroupSize);
        compute.Dispatch(0, threadGroups, 1, 1);

        boidBuffer.GetData(boidData);

        for (var i = 0; i < boids.Length; i++)
        {
            boids[i].avgFlockHeading = boidData[i].flockHeading;
            boids[i].centreOfFlockmates = boidData[i].flockCentre;
            boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            boids[i].UpdateBoid();
        }

        boidBuffer.Release();
    }

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size => sizeof(float) * 3 * 5 + sizeof(int);
    }
}