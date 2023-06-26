using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateAlignmentJob : IJobParallelFor
{
    [ReadOnly] public float alignmentWeight;
    [ReadOnly] public float maxSpeed;
    [ReadOnly] public float maxSteerForce;
    [ReadOnly] public NativeArray<float3> boidVelocities;
    [ReadOnly] public NativeArray<float3> boidFlockDirections;
        
    [WriteOnly] public NativeArray<float3> alignmentValues;

    public void Execute(int index)
    {
        float3 alignmentValue = BoidsMathUtility.GetClampedDirection(boidFlockDirections[index],
            boidVelocities[index], maxSpeed, maxSteerForce) * alignmentWeight;

        alignmentValues[index] = alignmentValue;
    }
}