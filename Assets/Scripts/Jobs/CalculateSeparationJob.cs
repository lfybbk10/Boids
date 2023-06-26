using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateSeparationJob : IJobParallelFor
{
    [ReadOnly] public float separationWeight;
    [ReadOnly] public float maxSpeed;
    [ReadOnly] public float maxSteerForce;
    [ReadOnly] public NativeArray<float3> boidVelocities;
    [ReadOnly] public NativeArray<float3> boidAverageAvoidances;
    [WriteOnly] public NativeArray<float3> separationValues;

    public void Execute(int index)
    {
        float3 separationValue = BoidsMathUtility.GetClampedDirection(boidAverageAvoidances[index],
            boidVelocities[index], maxSpeed, maxSteerForce) * separationWeight;

        separationValues[index] = separationValue;
    }
}