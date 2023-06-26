using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateTargetOffsetJob : IJobParallelFor
{
    [ReadOnly] public float targetWeight;
    [ReadOnly] public float maxSpeed;
    [ReadOnly] public float maxSteerForce;
    [ReadOnly] public NativeArray<float3> boidPositions;
    [ReadOnly] public NativeArray<float3> boidTargetPositions;
    [ReadOnly] public NativeArray<float3> boidVelocities;
        
    [WriteOnly] public NativeArray<float3> targetOffsetValues;

    public void Execute(int index)
    {
        float3 targetOffsetValue = BoidsMathUtility.GetClampedDirection(
            boidTargetPositions[index] - boidPositions[index], boidVelocities[index], 
            maxSpeed, maxSteerForce) * targetWeight;

        targetOffsetValues[index] = targetOffsetValue;
    }
}