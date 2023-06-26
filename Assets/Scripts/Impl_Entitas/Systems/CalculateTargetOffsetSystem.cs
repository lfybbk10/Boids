using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateTargetOffsetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly BoidSettings _boidSettings;

        public CalculateTargetOffsetSystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Position,
                GameMatcher.Velocity,
                GameMatcher.FollowingTarget,
                GameMatcher.TargetOffset));

            _boidSettings = settings;
        }

        public void Execute()
        {
            NativeArray<float3> _targetOffsets = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidTargetPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidPositions[entityIndex] = e.position.value;
                _boidTargetPositions[entityIndex] = e.followingTarget.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateTargetOffsetJob targetOffsetJob = new()
            {
                targetWeight = _boidSettings.targetWeight,
                maxSpeed = _boidSettings.maxSpeed,
                maxSteerForce = _boidSettings.maxSteerForce,
                boidPositions = _boidPositions,
                boidTargetPositions = _boidTargetPositions,
                boidVelocities = _boidVelocities,
                targetOffsetValues = _targetOffsets
            };

            JobHandle jobHandle = targetOffsetJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceTargetOffset(_targetOffsets[entityIndex]);

                entityIndex++;
            }

            _targetOffsets.Dispose();
            _boidPositions.Dispose();
            _boidTargetPositions.Dispose();
            _boidVelocities.Dispose();
        }
    }
}