using Entitas;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateCohesionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly BoidSettings _boidSettings;

        public CalculateCohesionSystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Position,
                GameMatcher.Velocity,
                GameMatcher.FlockmateNumber,
                GameMatcher.FlockCenter,
                GameMatcher.Cohesion));

            _boidSettings = settings;
        }

        public void Execute()
        {
            NativeArray<float3> _cohesionValues = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidFlockCenters = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidPositions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _boidVelocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);

            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _boidFlockCenters[entityIndex] = e.flockCenter.value / e.flockmateNumber.value;
                _boidPositions[entityIndex] = e.position.value;
                _boidVelocities[entityIndex] = e.velocity.value;

                entityIndex++;
            }

            CalculateCohesionJob calculateCohesionJob = new()
            {
                cohesionWeight = _boidSettings.cohesionWeight,
                maxSpeed = _boidSettings.maxSpeed,
                maxSteerForce = _boidSettings.maxSteerForce,
                boidPositions = _boidPositions,
                boidVelocities = _boidVelocities,
                boidFlockCenters = _boidFlockCenters,
                cohesionValues = _cohesionValues
            };

            JobHandle jobHandle = calculateCohesionJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();

            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                if (e.flockmateNumber.value == 0)
                {
                    _cohesionValues[entityIndex] = float3.zero;
                    ;
                }

                e.ReplaceCohesion(_cohesionValues[entityIndex]);

                entityIndex++;
            }

            _cohesionValues.Dispose();
            _boidFlockCenters.Dispose();
            _boidPositions.Dispose();
            _boidVelocities.Dispose();
        }
    }
}