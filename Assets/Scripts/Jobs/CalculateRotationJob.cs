using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateRotationJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> boidVelocities;
    [WriteOnly] public NativeArray<float3> boidRotations;
        
    public void Execute(int index)
    {
        float3 dir = boidVelocities[index] / math.length(boidVelocities[index]);
        boidRotations[index] = math.forward(quaternion.LookRotationSafe(dir, math.up()));
    }
}