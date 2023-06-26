using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateCohesionJob : IJobParallelFor
{
    [ReadOnly] public float cohesionWeight;
    [ReadOnly] public float maxSpeed;
    [ReadOnly] public float maxSteerForce;
    [ReadOnly] public NativeArray<float3> boidPositions;
    [ReadOnly] public NativeArray<float3> boidVelocities;
    [ReadOnly] public NativeArray<float3> boidFlockCenters;
    [WriteOnly] public NativeArray<float3> cohesionValues;

    public void Execute(int index)
    {
        float3 cohesionValue = BoidsMathUtility.GetClampedDirection(
            boidFlockCenters[index] - boidPositions[index], boidVelocities[index],
            maxSpeed, maxSteerForce) * cohesionWeight;

        cohesionValues[index] = cohesionValue;
    }
}