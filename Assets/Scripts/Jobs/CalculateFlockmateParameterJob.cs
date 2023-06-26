using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CalculateFlockmateParameterJob : IJobParallelFor
{
    [ReadOnly] public float perceptionRadius;
    [ReadOnly] public float avoidanceRadius;
    [ReadOnly] public NativeArray<float3> boidPositions;
    [ReadOnly] public NativeArray<float3> boidRotations;
        
    [WriteOnly] public NativeArray<float3> flockHeadings;
    [WriteOnly] public NativeArray<int> flockmateNumbers;
    [WriteOnly] public NativeArray<float3> flockCenters;
    [WriteOnly] public NativeArray<float3> averageAvoidances;
        
    public void Execute(int index)
    {
        float3 currentPosition = boidPositions[index];
            
        int flockmates = 0;
        float3 flockHeading = float3.zero;
        float3 flockmatesCenter = float3.zero;
        float3 avoidanceHeading = float3.zero;
            
        for (int i = 0; i < boidPositions.Length; i++)
        {
            if (i != index)
            {
                float3 otherPosition = boidPositions[i];
                float sqrDst = math.distancesq(currentPosition, otherPosition);

                if (sqrDst < perceptionRadius * perceptionRadius)
                {
                    flockmates++;
                    flockHeading += boidRotations[i];
                    flockmatesCenter += otherPosition;

                    if (sqrDst < avoidanceRadius * avoidanceRadius)
                    {
                        avoidanceHeading -= (otherPosition - currentPosition) / sqrDst;
                    }
                }
            }
        }
            
        flockmateNumbers[index] = flockmates;
        flockHeadings[index] = flockHeading;
        flockCenters[index] = flockmatesCenter;
        averageAvoidances[index] = avoidanceHeading;
    }
}