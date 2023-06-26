using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CheckCollisionTrajectorySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly float _boundsRadius;
        private readonly float _collisionAvoidanceDistance;

        public CheckCollisionTrajectorySystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Position,
                GameMatcher.Rotation));

            _boundsRadius = settings.boundsRadius;
            _collisionAvoidanceDistance = settings.collisionAvoidDst;
        }

        public void Execute()
        {
            NativeArray<bool> _boidCollisionTrajectoryStatuses =
                new NativeArray<bool>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidRotations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidRotations[entityIndex] = e.rotation.value;

                entityIndex++;
            }

            CheckCollisionTrajectoryJob checkCollisionTrajectoryJob = new()
            {
                boundsRadius = _boundsRadius,
                collisionAvoidanceDistance = _collisionAvoidanceDistance,
                boidPositions = _boidPositions,
                boidRotations = _boidRotations,
                collisionTrajectoryStatuses = _boidCollisionTrajectoryStatuses
            };

            JobHandle jobHandle = checkCollisionTrajectoryJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceIsOnCollisionTrajectory(_boidCollisionTrajectoryStatuses[entityIndex]);

                entityIndex++;
            }

            _boidCollisionTrajectoryStatuses.Dispose();
            _boidPositions.Dispose();
            _boidRotations.Dispose();
        }
    }
}