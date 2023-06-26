using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateAlignmentSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly BoidSettings _boidSettings;

        public CalculateAlignmentSystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Velocity,
                GameMatcher.AverageFlockDirection,
                GameMatcher.Alignment));

            _boidSettings = settings;
        }

        public void Execute()
        {
            NativeArray<float3> _alignmentValues = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidAverageFlockDirections =
                new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidAverageFlockDirections[entityIndex] = e.averageFlockDirection.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateAlignmentJob calculateAlignmentJob = new()
            {
                alignmentWeight = _boidSettings.alignmentWeight,
                maxSpeed = _boidSettings.maxSpeed,
                maxSteerForce = _boidSettings.maxSteerForce,
                boidVelocities = _boidVelocities,
                boidFlockDirections = _boidAverageFlockDirections,
                alignmentValues = _alignmentValues
            };

            JobHandle jobHandle = calculateAlignmentJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.flockmateNumber.value == 0)
                {
                    _alignmentValues[entityIndex] = float3.zero;
                    ;
                }

                e.ReplaceAlignment(_alignmentValues[entityIndex]);

                entityIndex++;
            }

            _alignmentValues.Dispose();
            _boidAverageFlockDirections.Dispose();
            _boidVelocities.Dispose();
        }
    }
}