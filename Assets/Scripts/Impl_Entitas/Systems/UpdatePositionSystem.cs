using UnityEngine;
using Entitas;

namespace Boids.Entitas
{
    public class UpdatePositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        public UpdatePositionSystem(GameContext context)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Position,
                GameMatcher.Velocity));
        }

        public void Execute()
        {
            /*NativeArray<float3> _positions = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            NativeArray<float3> _velocities = new NativeArray<float3>(_boidsGroup.count, Allocator.TempJob);
            float _deltaTime = Time.deltaTime;
            
            int entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                _positions[entityIndex] = new float3(e.position.x, e.position.y, e.position.z);
                _velocities[entityIndex] = new float3(e.velocity.x, e.velocity.y, e.velocity.z);
    
                entityIndex++;
            }
            
            CalculatePositionJob calculatePositionJob = new()
            {
                boidPositions = _positions,
                boidVelocities = _velocities,
                deltaTime = _deltaTime
            };
            
            JobHandle jobHandle = calculatePositionJob.Schedule(_boidsGroup.count, 32);
            jobHandle.Complete();
            
            entityIndex = 0;
            foreach (GameEntity e in _boidsGroup)
            {
                e.ReplacePosition(_positions[entityIndex].x,
                    _positions[entityIndex].y,
                    _positions[entityIndex].z);
                
                entityIndex++;
            }
    
            _positions.Dispose();
            _velocities.Dispose();*/

            foreach (GameEntity e in _boidsGroup)
            {
                e.position.value += e.velocity.value * Time.deltaTime;

            }
        }

        /*[BurstCompile]
        private struct CalculatePositionJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<float3> boidVelocities;
            [ReadOnly] public float deltaTime;
    
            public NativeArray<float3> boidPositions;
            
            public void Execute(int index)
            {
                boidPositions[index] += boidVelocities[index] * deltaTime;
            }
        }*/
    }
}