using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateAccelerationJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> boidTargetOffsets;
    [ReadOnly] public NativeArray<float3> boidCohesions;
    [ReadOnly] public NativeArray<float3> boidAlignments;
    [ReadOnly] public NativeArray<float3> boidSeparations;
    [ReadOnly] public NativeArray<float3> boidCollisionAvoidances;

    [WriteOnly] public NativeArray<float3> accelerations;
        
    public void Execute(int index)
    {
        float3 acceleration = boidTargetOffsets[index];
            
        acceleration += boidCohesions[index];
        acceleration += boidSeparations[index];
        acceleration += boidAlignments[index];
        acceleration += boidCollisionAvoidances[index];
            
        accelerations[index] = acceleration;
    }
}