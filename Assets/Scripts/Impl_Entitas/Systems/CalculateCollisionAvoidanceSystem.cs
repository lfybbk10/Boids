using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateCollisionAvoidanceSystem : IExecuteSystem
    {
        private const int AVOIDANCE_RAYCAST_DIRECTION_COUNT = 300;

        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly float _collisionAvoidanceWeight;
        private readonly float _boundsRadius;
        private readonly float _collisionAvoidanceDistance;
        private readonly float _maxSpeed;
        private readonly float _maxSteerForce;

        private readonly float3[] _avoidanceRaycastDirectionsArray;

        public CalculateCollisionAvoidanceSystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Position,
                GameMatcher.Rotation,
                GameMatcher.Velocity,
                GameMatcher.IsOnCollisionTrajectory,
                GameMatcher.Avoidance));

            _collisionAvoidanceWeight = settings.avoidCollisionWeight;
            _maxSpeed = settings.maxSpeed;
            _maxSteerForce = settings.maxSteerForce;
            _boundsRadius = settings.boundsRadius;
            _collisionAvoidanceDistance = settings.collisionAvoidDst;
            _avoidanceRaycastDirectionsArray =
                BoidsMathUtility.GetAvoidanceRayDirections(AVOIDANCE_RAYCAST_DIRECTION_COUNT);
        }

        public void Execute()
        {
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidRotations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<bool> _boidIsOnCollisionTrajectoryStatuses =
                new NativeArray<bool>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _collisionAvoidances = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _avoidanceRaycastDirections = new NativeArray<float3>(_avoidanceRaycastDirectionsArray,
                Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidRotations[entityIndex] = e.rotation.value;
                _boidVelocities[entityIndex] = e.velocity.value;
                _boidIsOnCollisionTrajectoryStatuses[entityIndex] = e.isOnCollisionTrajectory.value;

                entityIndex++;
            }

            CalculateCollisionAvoidanceJob calculateCollisionAvoidanceJob = new()
            {
                collisionAvoidanceWeight = _collisionAvoidanceWeight,
                maxSpeed = _maxSpeed,
                maxSteerForce = _maxSteerForce,
                boundsRadius = _boundsRadius,
                collisionAvoidanceDistance = _collisionAvoidanceDistance,
                boidPositions = _boidPositions,
                boidRotations = _boidRotations,
                boidVelocities = _boidVelocities,
                boidIsOnCollisionTrajectoryStatuses = _boidIsOnCollisionTrajectoryStatuses,
                avoidanceRaycastDirections = _avoidanceRaycastDirections,
                collisionAvoidances = _collisionAvoidances
            };

            JobHandle jobHandle = calculateCollisionAvoidanceJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (!e.isOnCollisionTrajectory.value)
                {
                    _collisionAvoidances[entityIndex] = float3.zero;
                }

                e.ReplaceAvoidance(_collisionAvoidances[entityIndex]);

                entityIndex++;
            }

            _boidPositions.Dispose();
            _boidRotations.Dispose();
            _boidVelocities.Dispose();
            _boidIsOnCollisionTrajectoryStatuses.Dispose();
            _collisionAvoidances.Dispose();
            _avoidanceRaycastDirections.Dispose();
        }
    }
}