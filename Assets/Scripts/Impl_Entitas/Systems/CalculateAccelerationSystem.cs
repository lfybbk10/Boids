using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateAccelerationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        public CalculateAccelerationSystem(GameContext context)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Acceleration,
                GameMatcher.TargetOffset,
                GameMatcher.Cohesion,
                GameMatcher.Separation,
                GameMatcher.Alignment,
                GameMatcher.Avoidance));
        }

        public void Execute()
        {
            NativeArray<float3> _accelerations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _targetOffsets = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _cohesions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _alignments = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _separations = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _collisionAvoidances = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _targetOffsets[entityIndex] = e.targetOffset.value;
                _cohesions[entityIndex] = e.cohesion.value;
                _alignments[entityIndex] = e.alignment.value;
                _separations[entityIndex] = e.separation.value;
                _collisionAvoidances[entityIndex] = e.avoidance.value;

                entityIndex++;
            }

            CalculateAccelerationJob targetOffsetJob = new()
            {
                boidTargetOffsets = _targetOffsets,
                boidCohesions = _cohesions,
                boidAlignments = _alignments,
                boidSeparations = _separations,
                boidCollisionAvoidances = _collisionAvoidances,
                accelerations = _accelerations
            };

            JobHandle jobHandle = targetOffsetJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplaceAcceleration(_accelerations[entityIndex]);

                entityIndex++;
            }

            _accelerations.Dispose();
            _targetOffsets.Dispose();
            _cohesions.Dispose();
            _alignments.Dispose();
            _separations.Dispose();
            _collisionAvoidances.Dispose();
        }
    }
}