using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
    public struct CalculateCollisionAvoidanceJob : IJobParallelFor
    {
        [ReadOnly] public float collisionAvoidanceWeight;
        [ReadOnly] public float maxSpeed;
        [ReadOnly] public float maxSteerForce;
        [ReadOnly] public float boundsRadius;
        [ReadOnly] public float collisionAvoidanceDistance;
        [ReadOnly] public NativeArray<float3> boidPositions;
        [ReadOnly] public NativeArray<float3> boidRotations;
        [ReadOnly] public NativeArray<float3> boidVelocities;
        [ReadOnly] public NativeArray<bool> boidIsOnCollisionTrajectoryStatuses;
        [ReadOnly] public NativeArray<float3> avoidanceRaycastDirections;

        [WriteOnly] public NativeArray<float3> collisionAvoidances;

        public void Execute(int index)
        {
            if (!boidIsOnCollisionTrajectoryStatuses[index])
            {
                collisionAvoidances[index] = float3.zero;
                return;
            }
            
            float3 avoidanceTrajectory = boidRotations[index];
            
            for (int i = 0; i < boidPositions.Length; i++)
            {
                if (i != index)
                {
                    for (int j = 0; j < avoidanceRaycastDirections.Length; j++)
                    {
                        float3 rayDirection = math.mul(quaternion.LookRotationSafe(boidRotations[index], math.up()), 
                            avoidanceRaycastDirections[j]);
                        float3 rayProjection = BoidsMathUtility.GetClosestPointOnRay(boidPositions[index],
                            rayDirection, boidPositions[i]);

                        if (math.distance(boidPositions[index], rayProjection) >= collisionAvoidanceDistance &&
                            math.distance(boidPositions[i], rayProjection) < boundsRadius)
                        {
                            collisionAvoidances[index] = BoidsMathUtility.GetClampedDirection(
                                avoidanceTrajectory, boidVelocities[index], 
                                maxSpeed, maxSteerForce) * collisionAvoidanceWeight;
                            return;
                        }
                    }
                }
            }

            collisionAvoidances[index] = BoidsMathUtility.GetClampedDirection(
                avoidanceTrajectory, boidVelocities[index], maxSpeed, maxSteerForce) * collisionAvoidanceWeight;
        }
    }