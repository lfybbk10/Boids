using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CheckCollisionTrajectoryJob : IJobParallelFor
{
    [ReadOnly] public float boundsRadius;
    [ReadOnly] public float collisionAvoidanceDistance;
    [ReadOnly] public NativeArray<float3> boidPositions;
    [ReadOnly] public NativeArray<float3> boidRotations;

    [WriteOnly] public NativeArray<bool> collisionTrajectoryStatuses;

    public void Execute(int index)
    {
        bool foundCollisionTrajectory = false;
            
        for (int i = 0; i < boidPositions.Length; i++)
        {
            if (i != index && !foundCollisionTrajectory)
            {
                float3 rayProjection = BoidsMathUtility.GetClosestPointOnRay(boidPositions[index],
                    boidRotations[index], boidPositions[i]);

                if (math.distance(boidPositions[index], rayProjection) < collisionAvoidanceDistance &&
                    math.distance(boidPositions[i], rayProjection) < boundsRadius)
                {
                    foundCollisionTrajectory = true;
                }
            }
        }

        collisionTrajectoryStatuses[index] = foundCollisionTrajectory;
    }
}