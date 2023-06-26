using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateSeparationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly BoidSettings _boidSettings;

        public CalculateSeparationSystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Velocity,
                GameMatcher.AverageAvoidance,
                GameMatcher.Separation));

            _boidSettings = settings;
        }

        public void Execute()
        {
            NativeArray<float3> _separationValues = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidAverageAvoidances = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidAverageAvoidances[entityIndex] = e.averageAvoidance.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateSeparationJob calculateSeparationJob = new()
            {
                separationWeight = _boidSettings.separationWeight,
                maxSpeed = _boidSettings.maxSpeed,
                maxSteerForce = _boidSettings.maxSteerForce,
                boidVelocities = _boidVelocities,
                boidAverageAvoidances = _boidAverageAvoidances,
                separationValues = _separationValues
            };

            JobHandle jobHandle = calculateSeparationJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.flockmateNumber.value == 0)
                {
                    _separationValues[entityIndex] = float3.zero;
                    ;
                }

                e.ReplaceSeparation(_separationValues[entityIndex]);

                entityIndex++;
            }

            _separationValues.Dispose();
            _boidAverageAvoidances.Dispose();
            _boidVelocities.Dispose();
        }
    }
}